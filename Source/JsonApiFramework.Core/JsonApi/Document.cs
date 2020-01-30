﻿// Copyright (c) 2015–Present Scott McDonald. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.md in the project root for license information.

using System.Collections.Generic;

using JsonApiFramework.Json;

using Newtonsoft.Json;

namespace JsonApiFramework.JsonApi
{
    /// <summary>
    /// Represents a json:api compliant document.
    /// </summary>
    /// <remarks> 
    /// A JSON object MUST be at the root of every JSON API response
    /// containing data. This object defines a document's "top level".
    ///
    /// The document's "primary data" is a representation of the resource,
    /// collection of resources, or resource relationship primarily targeted
    /// by a request.
    ///
    /// A document MUST contain at least one of the following top-level
    /// members:
    /// - primary data
    /// - array of error objects
    /// - meta object
    ///
    /// Primary data MUST appear under a top-level key named "data". Primary
    /// data MUST be either a single resource or resource identifier object,
    /// an array of resource or resource identifier objects.
    /// </remarks>
    /// <see cref="http://jsonapi.org"/>
    [JsonConverter(typeof(DocumentConverter))]
    [JsonObject(MemberSerialization.OptIn)]
    public class Document : JsonObject
        , IGetJsonApiVersion
        , IGetLinks
        , IGetMeta
        , ISetJsonApiVersion
        , ISetLinks
        , ISetMeta
    {
        // PUBLIC PROPERTIES ////////////////////////////////////////////////
        #region JSON Properties
        /// <summary>
        /// JsonApi object describing the client/server understanding/implementation
        /// of JSON API.
        /// </summary>
        [JsonProperty(Keywords.JsonApi)] public JsonApiVersion JsonApiVersion { get; set; }

        /// <summary>
        /// Meta object that contains non-standard meta information.
        /// </summary>
        [JsonProperty(Keywords.Meta)] public Meta Meta { get; set; }

        /// <summary>
        /// URLs related to the primary data of a document.
        /// </summary>
        /// <remarks>
        /// The top-level links object MAY contain the following members:
        /// <list type="bullet">
        /// <item><description>"self" - the URL that generated the current response document.</description></item>
        /// <item><description>"related" - a related resource URL when the primary data represents a resource relationship.</description></item>
        /// <item><description>Pagination links for the primary data.</description></item>
        /// </list>
        /// </remarks>
        [JsonProperty(Keywords.Links)] public Links Links { get; set; }
        #endregion

        // PUBLIC METHODS ///////////////////////////////////////////////////
        #region Object Overrides
        public override string ToString()
        { return TypeName; }
        #endregion

        #region Document Overrides
        /// <summary>Adds an error to this errors document.</summary>
        public virtual void AddError(Error error)
        { throw new JsonApiException(CoreErrorStrings.DocumentNotErrorsDocumentTitle, CoreErrorStrings.DocumentDoesNotContainErrorsMemberDetail); }

        /// <summary>Adds an error collection to this errors document.</summary>
        public virtual void AddErrors(IEnumerable<Error> errorCollection)
        { throw new JsonApiException(CoreErrorStrings.DocumentNotErrorsDocumentTitle, CoreErrorStrings.DocumentDoesNotContainErrorsMemberDetail); }

        /// <summary>Adds an included resource to this resource document or resource collection document.</summary>
        public virtual void AddIncludedResource(Resource resource)
        { throw new JsonApiException(CoreErrorStrings.DocumentNotResourceOrResourceCollectionDocumentTitle, CoreErrorStrings.DocumentDoesNotContainIncludedMemberDetail); }

        /// <summary>Adds an included resource collection to this resource document or resource collection document.</summary>
        public virtual void AddIncludedResources(IEnumerable<Resource> resourceCollection)
        { throw new JsonApiException(CoreErrorStrings.DocumentNotResourceOrResourceCollectionDocumentTitle, CoreErrorStrings.DocumentDoesNotContainIncludedMemberDetail); }

        /// <summary>Adds a primary resource to this resource collection document.</summary>
        public virtual void AddResource(Resource resource)
        { throw new JsonApiException(CoreErrorStrings.DocumentNotResourceCollectionDocumentTitle, CoreErrorStrings.DocumentDoesNotContainDataMemberAsResourceCollectionDetail); }

        /// <summary>Adds a primary resource collection to this resource collection document.</summary>
        public virtual void AddResourceRange(IEnumerable<Resource> resourceCollection)
        { throw new JsonApiException(CoreErrorStrings.DocumentNotResourceCollectionDocumentTitle, CoreErrorStrings.DocumentDoesNotContainDataMemberAsResourceCollectionDetail); }

        /// <summary>Adds a primary resource identifier to this resource identifier document or resource identifier collection document.</summary>
        public virtual void AddResourceIdentifier(ResourceIdentifier resourceIdentifier)
        { throw new JsonApiException(CoreErrorStrings.DocumentNotResourceIdentifierCollectionDocumentTitle, CoreErrorStrings.DocumentDoesNotContainDataMemberAsResourceIdentifierCollectionDetail); }

        /// <summary>Adds a primary resource identifier collection to this resource identifier collection document.</summary>
        public virtual void AddResourceIdentifierRange(IEnumerable<ResourceIdentifier> resourceIdentifierCollection)
        { throw new JsonApiException(CoreErrorStrings.DocumentNotResourceIdentifierCollectionDocumentTitle, CoreErrorStrings.DocumentDoesNotContainDataMemberAsResourceIdentifierCollectionDetail); }

        /// <summary>Gets the primary data from this document.</summary>
        public virtual object GetData()
        { throw new JsonApiException(CoreErrorStrings.DocumentNotResourceOrientedDocumentTitle, CoreErrorStrings.DocumentDoesNotContainDataMemberDetail); }

        /// <summary>Gets the document type of this document.</summary>
        public virtual DocumentType GetDocumentType()
        { return DocumentType.Document; }

        /// <summary>Gets the primary single resource from this document.</summary>
        public virtual Resource GetResource()
        { throw new JsonApiException(CoreErrorStrings.DocumentNotResourceDocumentTitle, CoreErrorStrings.DocumentDoesNotContainDataMemberAsResourceDetail); }

        /// <summary>Gets the primary array of resources from this document.</summary>
        public virtual IEnumerable<Resource> GetResourceCollection()
        { throw new JsonApiException(CoreErrorStrings.DocumentNotResourceCollectionDocumentTitle, CoreErrorStrings.DocumentDoesNotContainDataMemberAsResourceCollectionDetail); }

        /// <summary>Gets the primary single resource identifier from this document.</summary>
        public virtual ResourceIdentifier GetResourceIdentifier()
        { throw new JsonApiException(CoreErrorStrings.DocumentNotResourceIdentifierDocumentTitle, CoreErrorStrings.DocumentDoesNotContainDataMemberAsResourceIdentifierDetail); }

        /// <summary>Gets the primary array of resource identifiers from this document.</summary>
        public virtual IEnumerable<ResourceIdentifier> GetResourceIdentifierCollection()
        { throw new JsonApiException(CoreErrorStrings.DocumentNotResourceIdentifierCollectionDocumentTitle, CoreErrorStrings.DocumentDoesNotContainDataMemberAsResourceIdentifierCollectionDetail); }

        /// <summary>Gets the included array of resources from this document.</summary>
        public virtual IEnumerable<Resource> GetIncludedResources()
        { throw new JsonApiException(CoreErrorStrings.DocumentNotResourceOrResourceCollectionDocumentTitle, CoreErrorStrings.DocumentDoesNotContainIncludedMemberDetail); }

        /// <summary>Gets the errors from this document.</summary>
        public virtual IEnumerable<Error> GetErrors()
        { throw new JsonApiException(CoreErrorStrings.DocumentNotErrorsDocumentTitle, CoreErrorStrings.DocumentDoesNotContainErrorsMemberDetail); }

        /// <summary>Sets the primary resource on this resource document.</summary>
        public virtual void SetResource(Resource resource)
        { throw new JsonApiException(CoreErrorStrings.DocumentNotResourceDocumentTitle, CoreErrorStrings.DocumentDoesNotContainDataMemberAsResourceDetail); }

        /// <summary>Sets the primary resource identifier on this resource identifier document.</summary>
        public virtual void SetResourceIdentifier(ResourceIdentifier resourceIdentifier)
        { throw new JsonApiException(CoreErrorStrings.DocumentNotResourceIdentifierDocumentTitle, CoreErrorStrings.DocumentDoesNotContainDataMemberAsResourceIdentifierDetail); }
        #endregion

        // PUBLIC FIELDS ////////////////////////////////////////////////////
        #region Fields
        public static readonly Document Empty = new Document();
        #endregion

        // PRIVATE FIELDS ///////////////////////////////////////////////////
        #region Fields
        private static readonly string TypeName = typeof(Document).Name;
        #endregion
    }
}