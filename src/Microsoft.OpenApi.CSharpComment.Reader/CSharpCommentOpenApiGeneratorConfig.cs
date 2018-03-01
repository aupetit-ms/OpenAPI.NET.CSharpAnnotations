﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.OpenApi.CSharpComment.Reader.DocumentConfigFilters;
using Microsoft.OpenApi.CSharpComment.Reader.DocumentFilters;
using Microsoft.OpenApi.CSharpComment.Reader.OperationConfigFilters;
using Microsoft.OpenApi.CSharpComment.Reader.OperationFilters;
using Microsoft.OpenApi.CSharpComment.Reader.PostProcessingDocumentFilters;
using Microsoft.OpenApi.CSharpComment.Reader.PreprocessingOperationFilters;

namespace Microsoft.OpenApi.CSharpComment.Reader
{
    /// <summary>
    /// The class to store the configuration that will be used to generate the OpenAPI document from csharp
    /// documentation.
    /// </summary>
    public class CSharpCommentOpenApiGeneratorConfig
    {
        private static readonly CSharpCommentOpenApiGeneratorFilterConfig _defaultFilterConfig =
            new CSharpCommentOpenApiGeneratorFilterConfig(
                new List<IDocumentFilter>
                {
                    new AssemblyNameToInfoFilter(),
                    new UrlToServerFilter(),
                    new MemberSummaryToSchemaDescriptionFilter()
                },
                new List<IOperationFilter>
                {
                    new GroupToTagFilter(),
                    new ParamToParameterFilter(),
                    new ParamToRequestBodyFilter(),
                    new ResponseToResponseFilter(),
                    new RemarksToDescriptionFilter(),
                    new SummaryToSummaryFilter()
                }
             )
            {
                DocumentConfigFilters = new List<IDocumentConfigFilter>
                    {
                        new DocumentVariantAttributesFilter()
                    },
                OperationConfigFilters = new List<IOperationConfigFilter>
                    {
                        new CommonAnnotationFilter()
                    },
                PreProcessingOperationFilters = new List<IPreProcessingOperationFilter>
                    {
                        new ConvertAlternativeParamTagsFilter(),
                        new PopulateInAttributeFilter(),
                        new BranchOptionalPathParametersFilter()
                    },
                PostProcessingDocumentFilters = new List<IPostProcessingDocumentFilter>
                    {
                        new RemoveFailedGenerationOperationFilter()
                    }
            };

        /// <summary>
        /// Creates a new instance of <see cref="CSharpCommentOpenApiGeneratorConfig"/>.
        /// </summary>
        /// <param name="annotationXmlDocument">The XDocument representing the annotation xml.</param>
        /// <param name="assemblyPaths">The list of relative or absolute paths to the assemblies that will be used to
        /// reflect into the types provided in the xml.
        /// </param>
        /// <param name="openApiSpecificationVersion">The specification version of the OpenAPI document to generate.
        /// </param>
        public CSharpCommentOpenApiGeneratorConfig(
            XDocument annotationXmlDocument,
            IList<string> assemblyPaths,
            OpenApiSpecVersion openApiSpecificationVersion) 
            : this(
                  annotationXmlDocument,
                  assemblyPaths,
                  openApiSpecificationVersion,
                  _defaultFilterConfig)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="CSharpCommentOpenApiGeneratorConfig"/>.
        /// </summary>
        /// <param name="annotationXmlDocument">The XDocument representing the annotation xml.</param>
        /// <param name="assemblyPaths">The list of relative or absolute paths to the assemblies that will be used to
        /// reflect into the types provided in the xml.
        /// </param>
        /// <param name="openApiSpecificationVersion">The specification version of the OpenAPI document to generate.
        /// </param>
        /// <param name="cSharpCommentOpenApiGeneratorFilterConfig">The configuration encapsulating all the filters
        /// that will be applied while generating/processing OpenAPI document from C# comments.</param>
        public CSharpCommentOpenApiGeneratorConfig(
            XDocument annotationXmlDocument,
            IList<string> assemblyPaths,
            OpenApiSpecVersion openApiSpecificationVersion,
            CSharpCommentOpenApiGeneratorFilterConfig cSharpCommentOpenApiGeneratorFilterConfig )
        {
            AnnotationXmlDocument = annotationXmlDocument
                ?? throw new ArgumentNullException(nameof(annotationXmlDocument));

            AssemblyPaths = assemblyPaths
                ?? throw new ArgumentNullException(nameof(assemblyPaths));

            OpenApiSpecificationVersion = openApiSpecificationVersion;
            CSharpCommentOpenApiGeneratorFilterConfig = cSharpCommentOpenApiGeneratorFilterConfig;
        }

        /// <summary>
        /// The XDocument representing the advanced generation configuration.
        /// </summary>
        public XDocument AdvancedConfigurationXmlDocument { get; set; }

        /// <summary>
        /// The XDocument representing the annotation xml.
        /// </summary>
        public XDocument AnnotationXmlDocument { get; }

        /// <summary>
        /// The list of relative or absolute paths to the assemblies that will be used to reflect into the
        /// types provided in the xml.
        /// </summary>
        public IList<string> AssemblyPaths { get; }

        /// <summary>
        /// The configuration encapsulating all the filters that will be applied while generating/processing OpenAPI
        /// document from C# comments.
        /// </summary>
        public CSharpCommentOpenApiGeneratorFilterConfig CSharpCommentOpenApiGeneratorFilterConfig { get; }

        /// <summary>
        /// The format (YAML or JSON) of the OpenAPI document to generate.
        /// </summary>
        public OpenApiFormat OpenApiFormat { get; set; } = OpenApiFormat.Json;

        /// <summary>
        /// The specification version of the OpenAPI document to generate.
        /// </summary>
        public OpenApiSpecVersion OpenApiSpecificationVersion { get; }
    }
}