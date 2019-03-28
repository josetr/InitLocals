// Copyright (c) 2019 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace InitLocals.Tasks
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Cecil.Pdb;
    using Mono.Collections.Generic;

    public class InitLocals : Task
    {
        [Required]
        public string IntermediateAssembly { get; set; }

        [Required]
        public bool SignAssembly { get; set; }

        public string DebugType { get; set; }

        public string DebugSymbolsIntermediatePath { get; set; }

        public string KeyOriginatorFile { get; set; }

        public string AssemblyOriginatorKeyFile { get; set; }

        public override bool Execute()
        {
            DebugType = DebugType?.ToLowerInvariant();
            var bytes = File.ReadAllBytes(IntermediateAssembly);

            using (var stream = new MemoryStream(bytes))
            {
                var readerParameters = GetReaderParameters();
                try
                {
                    var module = ModuleDefinition.ReadModule(stream, readerParameters);
                    var moduleInitLocals = GetInitLocals(module.CustomAttributes);
                    var assemblyInitLocals = GetInitLocals(module.Assembly.CustomAttributes) ?? moduleInitLocals;

                    foreach (var type in module.Types)
                    {
                        var typeInitLocals = GetInitLocals(type.CustomAttributes) ?? assemblyInitLocals;

                        foreach (var method in type.Methods.Where(m => m.HasBody))
                        {
                            var methodInitLocals = GetInitLocals(method.CustomAttributes) ?? typeInitLocals;
                            method.Body.InitLocals = methodInitLocals.GetValueOrDefault(method.Body.InitLocals);
                            Log.LogMessage(method.Name + ".InitLocals = " + method.Body.InitLocals);
                        }
                    }

                    module.Write(IntermediateAssembly, GetWriterParameters(readerParameters));
                }
                finally
                {
                    readerParameters.SymbolStream?.Dispose();
                }
            }

            return true;
        }

        private static bool? GetInitLocals(Collection<CustomAttribute> customAttributes)
        {
            var initLocalsAttribute = customAttributes.FirstOrDefault(
                attr => attr.AttributeType.FullName == "InitLocalsAttribute");

            if (initLocalsAttribute == null)
                return null;

            if (initLocalsAttribute.ConstructorArguments.Count != 1)
                return null;

            return (bool)initLocalsAttribute.ConstructorArguments[0].Value;
        }

        private ReaderParameters GetReaderParameters()
        {
            var readerParameters = new ReaderParameters();

            switch (DebugType)
            {
                case "embedded":
                    readerParameters.ReadSymbols = true;
                    readerParameters.SymbolReaderProvider = new EmbeddedPortablePdbReaderProvider();
                    break;
                case "none":
                    break;
                default:
                    var stream = default(MemoryStream);
                    try
                    {
                        stream = new MemoryStream(File.ReadAllBytes(DebugSymbolsIntermediatePath));
                    }
                    catch (Exception e)
                    {
                        if (BuildEngine != null)
                            Log.LogMessage($"Failed reading debug program database {e.Message}");
                    }

                    if (stream != null)
                    {
                        readerParameters.ReadSymbols = true;
                        readerParameters.SymbolReaderProvider = new PdbReaderProvider();
                        readerParameters.SymbolStream = stream;
                    }

                    break;
            }

            return readerParameters;
        }

        private WriterParameters GetWriterParameters(ReaderParameters readerParameters)
        {
            var symbolWriterProvider = default(ISymbolWriterProvider);
            switch (readerParameters?.SymbolReaderProvider)
            {
                case PdbReaderProvider p:
                    symbolWriterProvider = new PdbWriterProvider();
                    break;
                case EmbeddedPortablePdbReaderProvider e:
                    symbolWriterProvider = new EmbeddedPortablePdbWriterProvider();
                    break;
            }

            var writeParameters = new WriterParameters
            {
                WriteSymbols = symbolWriterProvider != null,
                SymbolWriterProvider = symbolWriterProvider,
                SymbolStream = readerParameters.SymbolStream != null ? new FileStream(DebugSymbolsIntermediatePath, FileMode.OpenOrCreate) : null,
            };

            if (SignAssembly)
            {
                var path = KeyOriginatorFile ?? AssemblyOriginatorKeyFile;
                try
                {
                    writeParameters.StrongNameKeyPair = new StrongNameKeyPair(File.ReadAllBytes(path));
                }
                catch (Exception e)
                {
                    Log.LogMessage($"Failed signing assembly {e.Message})");
                }
            }

            return writeParameters;
        }
    }
}
