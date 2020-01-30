﻿// Copyright (c) 2015–Present Scott McDonald. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using JsonApiFramework.Internal.Dom;
using JsonApiFramework.JsonApi;
using JsonApiFramework.ServiceModel;

namespace JsonApiFramework.Server.Internal
{
    internal class RelationshipCollectionBuilder<TParentBuilder, TResource> : IRelationshipBuilder<TParentBuilder, TResource>
        where TParentBuilder : class
        where TResource : class, IResource
    {
        // PUBLIC METHODS ///////////////////////////////////////////////////
        #region IRelationshipsBuilder<TParentBuilder, TResource> Implementation
        public IRelationshipBuilder<TParentBuilder, TResource> SetMeta(Meta meta)
        {
            Contract.Requires(meta != null);

            var count = this.Count;
            for (var i = 0; i < count; ++i)
            {
                var domReadWriteRelationship = this.DomReadWriteRelationshipCollection[i];

                domReadWriteRelationship.SetDomReadOnlyMeta(meta);
            }

            return this;
        }

        public IRelationshipBuilder<TParentBuilder, TResource> SetMeta(IEnumerable<Meta> metaCollection)
        {
            Contract.Requires(metaCollection != null);

            var metaReadOnlyList = metaCollection.SafeToReadOnlyList();
            var metaReadOnlyListCount = metaReadOnlyList.Count;
            var domReadWriteRelationshipCollectionCount = this.DomReadWriteRelationshipCollection.Count;
            if (metaReadOnlyListCount != domReadWriteRelationshipCollectionCount)
            {
                var rel = this.Rel;
                var detail = ServerErrorStrings
                    .DocumentBuildExceptionDetailBuildRelationshipCollectionCountMismatch
                    .FormatWith(DomNodeType.Meta, domReadWriteRelationshipCollectionCount, rel, metaReadOnlyListCount);
                throw new DocumentBuildException(detail);
            }

            var count = this.Count;
            for (var i = 0; i < count; ++i)
            {
                var domReadWriteRelationship = this.DomReadWriteRelationshipCollection[i];
                var meta = metaReadOnlyList[i];

                domReadWriteRelationship.SetDomReadOnlyMeta(meta);
            }

            return this;
        }

        public IRelationshipLinksBuilder<IRelationshipBuilder<TParentBuilder, TResource>> Links()
        {
            var linksCollectionBuilder = new RelationshipLinksCollectionBuilder<IRelationshipBuilder<TParentBuilder, TResource>>(this, this.DomReadWriteRelationshipCollection, this.Rel);
            return linksCollectionBuilder;
        }

        public IRelationshipBuilder<TParentBuilder, TResource> SetData(IToOneResourceLinkage toOneResourceLinkage)
        {
            if (toOneResourceLinkage == null || toOneResourceLinkage.HasValue == false)
            {
                this.SetDataNullOrEmpty();
                return this;
            }

            var rel = this.Rel;
            var resourceType = this.ResourceType;
            var relationship = resourceType.GetRelationshipInfo(rel);

            var toCardinality = relationship.ToCardinality;
            var toClrType = relationship.ToClrType;
            var toResourceType = this.ServiceModel.GetResourceType(toClrType);

            switch (toCardinality)
            {
                case RelationshipCardinality.ToOne:
                {
                    var toApiResourceIdentifier = toOneResourceLinkage.CreateApiResourceIdentifier(toResourceType);
                    foreach (var domReadWriteRelationship in this.DomReadWriteRelationshipCollection)
                    {
                        domReadWriteRelationship.SetDomData(toApiResourceIdentifier, toClrType);
                    }
                }
                break;

                case RelationshipCardinality.ToMany:
                {
                    var clrResourceTypeName = resourceType.ClrType.Name;
                    var detail = InfrastructureErrorStrings.DocumentBuildExceptionDetailBuildToOneRelationshipResourceLinkageCardinalityMismatch
                                                           .FormatWith(clrResourceTypeName, rel);
                    throw new DocumentBuildException(detail);
                }

                default:
                {
                    var detail = InfrastructureErrorStrings.InternalErrorExceptionDetailUnknownEnumerationValue
                                                           .FormatWith(typeof(RelationshipCardinality).Name, toCardinality);
                    throw new InternalErrorException(detail);
                }
            }

            return this;
        }

        public IRelationshipBuilder<TParentBuilder, TResource> SetData(IEnumerable<IToOneResourceLinkage> toOneResourceLinkageCollection)
        {
            if (toOneResourceLinkageCollection == null)
            {
                this.SetDataNullOrEmpty();
                return this;
            }

            var rel = this.Rel;
            var resourceType = this.ResourceType;
            var relationship = resourceType.GetRelationshipInfo(rel);

            var toCardinality = relationship.ToCardinality;
            var toClrType = relationship.ToClrType;
            var toResourceType = this.ServiceModel.GetResourceType(toClrType);

            switch (toCardinality)
            {
                case RelationshipCardinality.ToOne:
                {
                    var toOneResourceLinkageList = toOneResourceLinkageCollection.SafeToReadOnlyList();
                    var toOneResourceLinkageListCount = toOneResourceLinkageList.Count;
                    var domReadWriteRelationshipCollectionCount = this.DomReadWriteRelationshipCollection.Count;
                    if (toOneResourceLinkageListCount != domReadWriteRelationshipCollectionCount)
                    {
                        var detail = ServerErrorStrings
                            .DocumentBuildExceptionDetailBuildRelationshipCollectionCountMismatch
                            .FormatWith(DomNodeType.Data, domReadWriteRelationshipCollectionCount, rel, toOneResourceLinkageListCount);
                        throw new DocumentBuildException(detail);
                    }

                    var count = this.Count;
                    for (var i = 0; i < count; ++i)
                    {
                        var domReadWriteRelationship = this.DomReadWriteRelationshipCollection[i];
                        var toOneResourceLinkage = toOneResourceLinkageList[i];
                        if (toOneResourceLinkage.HasValue)
                        {
                            var toApiResourceIdentifier = toOneResourceLinkage.CreateApiResourceIdentifier(toResourceType);
                            domReadWriteRelationship.SetDomData(toApiResourceIdentifier, toClrType);
                        }
                        else
                        {
                            domReadWriteRelationship.SetDomDataNull();
                        }
                    }
                }
                break;

                case RelationshipCardinality.ToMany:
                {
                    var clrResourceTypeName = resourceType.ClrType.Name;
                    var detail = InfrastructureErrorStrings.DocumentBuildExceptionDetailBuildToOneRelationshipResourceLinkageCardinalityMismatch
                                                           .FormatWith(clrResourceTypeName, rel);
                    throw new DocumentBuildException(detail);
                }

                default:
                {
                    var detail = InfrastructureErrorStrings.InternalErrorExceptionDetailUnknownEnumerationValue
                                                           .FormatWith(typeof(RelationshipCardinality).Name, toCardinality);
                    throw new InternalErrorException(detail);
                }
            }

            return this;
        }

        public IRelationshipBuilder<TParentBuilder, TResource> SetData(IToManyResourceLinkage toManyResourceLinkage)
        {
            if (toManyResourceLinkage == null || toManyResourceLinkage.HasValueCollection == false)
            {
                this.SetDataNullOrEmpty();
                return this;
            }

            var rel = this.Rel;
            var resourceType = this.ResourceType;
            var relationship = resourceType.GetRelationshipInfo(rel);

            var toCardinality = relationship.ToCardinality;
            var toClrType = relationship.ToClrType;
            var toResourceType = this.ServiceModel.GetResourceType(toClrType);

            switch (toCardinality)
            {
                case RelationshipCardinality.ToOne:
                {
                    var clrResourceTypeName = resourceType.ClrType.Name;
                    var detail = InfrastructureErrorStrings.DocumentBuildExceptionDetailBuildToManyRelationshipResourceLinkageCardinalityMismatch
                                                           .FormatWith(clrResourceTypeName, rel);
                    throw new DocumentBuildException(detail);
                }

                case RelationshipCardinality.ToMany:
                {
                    var toApiResourceIdentifierCollection = toManyResourceLinkage.CreateApiResourceIdentifierCollection(toResourceType);
                    foreach (var domReadWriteRelationship in this.DomReadWriteRelationshipCollection)
                    {
                        domReadWriteRelationship.SetDomDataCollection(toApiResourceIdentifierCollection, toClrType);
                    }
                }
                break;

                default:
                {
                    var detail = InfrastructureErrorStrings.InternalErrorExceptionDetailUnknownEnumerationValue
                                                           .FormatWith(typeof(RelationshipCardinality).Name, toCardinality);
                    throw new InternalErrorException(detail);
                }
            }

            return this;
        }

        public IRelationshipBuilder<TParentBuilder, TResource> SetData(IEnumerable<IToManyResourceLinkage> toManyResourceLinkageCollection)
        {
            if (toManyResourceLinkageCollection == null)
            {
                this.SetDataNullOrEmpty();
                return this;
            }

            var rel = this.Rel;
            var resourceType = this.ResourceType;
            var relationship = resourceType.GetRelationshipInfo(rel);

            var toCardinality = relationship.ToCardinality;
            var toClrType = relationship.ToClrType;
            var toResourceType = this.ServiceModel.GetResourceType(toClrType);

            switch (toCardinality)
            {
                case RelationshipCardinality.ToOne:
                {
                    var clrResourceTypeName = resourceType.ClrType.Name;
                    var detail = InfrastructureErrorStrings.DocumentBuildExceptionDetailBuildToManyRelationshipResourceLinkageCardinalityMismatch
                                                           .FormatWith(clrResourceTypeName, rel);
                    throw new DocumentBuildException(detail);
                }

                case RelationshipCardinality.ToMany:
                {
                    var toManyResourceLinkageList = toManyResourceLinkageCollection.SafeToReadOnlyList();
                    var toManyResourceLinkageListCount = toManyResourceLinkageList.Count;
                    var domReadWriteRelationshipCollectionCount = this.DomReadWriteRelationshipCollection.Count;
                    if (toManyResourceLinkageListCount != domReadWriteRelationshipCollectionCount)
                    {
                        var detail = ServerErrorStrings
                            .DocumentBuildExceptionDetailBuildRelationshipCollectionCountMismatch
                            .FormatWith(DomNodeType.Data, domReadWriteRelationshipCollectionCount, rel, toManyResourceLinkageListCount);
                        throw new DocumentBuildException(detail);
                    }

                    var count = this.Count;
                    for (var i = 0; i < count; ++i)
                    {
                        var domReadWriteRelationship = this.DomReadWriteRelationshipCollection[i];
                        var toManyResourceLinkage = toManyResourceLinkageList[i];
                        if (toManyResourceLinkage.HasValueCollection)
                        {
                            var toApiResourceIdentifierCollection = toManyResourceLinkage.CreateApiResourceIdentifierCollection(toResourceType);
                            domReadWriteRelationship.SetDomDataCollection(toApiResourceIdentifierCollection, toClrType);
                        }
                        else
                        {
                            domReadWriteRelationship.SetDomDataCollectionEmpty();
                        }
                    }
                }
                break;

                default:
                {
                    var detail = InfrastructureErrorStrings.InternalErrorExceptionDetailUnknownEnumerationValue
                                                           .FormatWith(typeof(RelationshipCardinality).Name, toCardinality);
                    throw new InternalErrorException(detail);
                }
            }

            return this;
        }

        public TParentBuilder RelationshipEnd()
        {
            var parentBuilder = this.ParentBuilder;
            return parentBuilder;
        }
        #endregion

        // INTERNAL CONSTRUCTORS ////////////////////////////////////////////
        #region Constructors
        internal RelationshipCollectionBuilder(TParentBuilder parentBuilder, IServiceModel serviceModel, IReadOnlyList<DomReadWriteRelationships> domReadWriteRelationshipsCollection, IReadOnlyList<TResource> clrResourceCollection, string rel, Func<TResource, bool> predicate)
        {
            Contract.Requires(parentBuilder != null);
            Contract.Requires(serviceModel != null);
            Contract.Requires(domReadWriteRelationshipsCollection != null);
            Contract.Requires(clrResourceCollection != null);
            Contract.Requires(String.IsNullOrWhiteSpace(rel) == false);

            var domReadWriteRelationshipsCollectionCount = domReadWriteRelationshipsCollection.Count;
            var clrResourceCollectionCount = clrResourceCollection.Count;
            if (clrResourceCollectionCount != domReadWriteRelationshipsCollectionCount)
            {
                var detail = ServerErrorStrings
                    .InternalErrorExceptionDetailCollectionCountMismatch
                    .FormatWith("DOM read-write relationships collection", domReadWriteRelationshipsCollectionCount, "CLR resource collection", clrResourceCollectionCount);
                throw new InternalErrorException(detail);
            }

            this.ParentBuilder = parentBuilder;

            this.ServiceModel = serviceModel;

            var resourceType = serviceModel.GetResourceType<TResource>();
            this.ResourceType = resourceType;

            this.Rel = rel;

            var domReadWriteRelationshipCollection = new List<DomReadWriteRelationship>();
            var clrResourceCollectionFiltered = new List<TResource>();

            var count = clrResourceCollectionCount;
            for (var i = 0; i < count; ++i)
            {
                var clrResource = clrResourceCollection[i];
                var canAddRelationship = predicate == null || predicate(clrResource);
                if (canAddRelationship == false)
                    continue;

                var domReadWriteRelationships = domReadWriteRelationshipsCollection[i];
                var domReadWriteRelationship = domReadWriteRelationships.AddDomReadWriteRelationship(rel);
                domReadWriteRelationshipCollection.Add(domReadWriteRelationship);

                clrResourceCollectionFiltered.Add(clrResource);
            }

            this.DomReadWriteRelationshipCollection = domReadWriteRelationshipCollection;
            this.ClrResourceCollection = clrResourceCollectionFiltered;
        }
        #endregion

        // PRIVATE PROPERTIES ///////////////////////////////////////////////
        #region Properties
        private TParentBuilder ParentBuilder { get; }
        private IServiceModel ServiceModel { get; }
        private IResourceType ResourceType { get; }
        private string Rel { get; }
        private int Count => this.ClrResourceCollection.Count;
        private IReadOnlyList<DomReadWriteRelationship> DomReadWriteRelationshipCollection { get; }
        private IReadOnlyList<TResource> ClrResourceCollection { get; }
        #endregion

        // PRIVATE METHODS //////////////////////////////////////////////////
        #region Methods
        private void SetDataNullOrEmpty()
        {
            var rel = this.Rel;
            var resourceType = this.ResourceType;
            var relationship = resourceType.GetRelationshipInfo(rel);

            var toCardinality = relationship.ToCardinality;
            switch (toCardinality)
            {
                case RelationshipCardinality.ToOne:
                {
                    foreach (var domReadWriteRelationship in this.DomReadWriteRelationshipCollection)
                    {
                        domReadWriteRelationship.SetDomDataNull();
                    }
                }
                break;

                case RelationshipCardinality.ToMany:
                {
                    foreach (var domReadWriteRelationship in this.DomReadWriteRelationshipCollection)
                    {
                        domReadWriteRelationship.SetDomDataCollectionEmpty();
                    }
                }
                break;

                default:
                {
                    var detail = InfrastructureErrorStrings.InternalErrorExceptionDetailUnknownEnumerationValue
                                                           .FormatWith(typeof(RelationshipCardinality).Name, toCardinality);
                    throw new InternalErrorException(detail);
                }
            }
        }
        #endregion
    }
}
