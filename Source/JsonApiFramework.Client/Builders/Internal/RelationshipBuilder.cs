// Copyright (c) 2015–Present Scott McDonald. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.md in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.Contracts;

using JsonApiFramework.Internal;
using JsonApiFramework.Internal.Dom;
using JsonApiFramework.Internal.Tree;
using JsonApiFramework.JsonApi;
using JsonApiFramework.ServiceModel;

namespace JsonApiFramework.Client.Internal
{
    internal class RelationshipBuilder<TParentBuilder, TResource> : RelationshipBuilderBase<TResource>, IRelationshipBuilder<TParentBuilder, TResource>
        where TParentBuilder : class
        where TResource : class, IResource
    {
        // PUBLIC METHODS ///////////////////////////////////////////////////
        #region IRelationshipBuilder<TParentBuilder, TResource> Implementation
        public IRelationshipBuilder<TParentBuilder, TResource> SetMeta(Meta meta)
        {
            base.SetMetaInternal(meta);
            return this;
        }

        public IRelationshipBuilder<TParentBuilder, TResource> SetData(IToOneResourceLinkage toOneResourceLinkage)
        {
<<<<<<< HEAD
            base.SetDataInternal(toOneResourceLinkage);
=======
            var rel = this.Rel;
            var resourceType = this.ResourceType;
            var relationship = resourceType.GetRelationshipInfo(rel);

            var toCardinality = relationship.ToCardinality;
            switch (toCardinality)
            {
                case RelationshipCardinality.ToOne:
                {
                    var domData = this.DomReadWriteRelationship.SetDomDataNull();
                }
                break;

                case RelationshipCardinality.ToMany:
                {
                    var domDataCollection = this.DomReadWriteRelationship.SetDomDataCollectionEmpty();
                }
                break;

                default:
                {
                    var detail = InfrastructureErrorStrings.InternalErrorExceptionDetailUnknownEnumerationValue
                                                           .FormatWith(typeof(RelationshipCardinality).Name, toCardinality);
                    throw new InternalErrorException(detail);
                }
            }

>>>>>>> 2cc26cdda1ac11d423461c703d9bc6adb524bb5c
            return this;
        }

        public IRelationshipBuilder<TParentBuilder, TResource> SetData(IToManyResourceLinkage toManyResourceLinkage)
        {
<<<<<<< HEAD
            base.SetDataInternal(toManyResourceLinkage);
=======
            // Build a JSON API resource identifier from the given relation name and CLR related resource identifier.
            var rel = this.Rel;
            var resourceType = this.ResourceType;
            var relationship = resourceType.GetRelationshipInfo(rel);

            var toClrType = relationship.ToClrType;

            var toResourceType = this.ServiceModel.GetResourceType(toClrType);
            var toApiResourceIdentifier = toResourceType.CreateApiResourceIdentifier(clrResourceId);

            var toCardinality = relationship.ToCardinality;
            switch (toCardinality)
            {
                case RelationshipCardinality.ToOne:
                {
                    var domData = toApiResourceIdentifier != null
                        ? this.DomReadWriteRelationship.SetDomData(toApiResourceIdentifier, toClrType)
                        : this.DomReadWriteRelationship.SetDomDataNull();
                }
                break;

                case RelationshipCardinality.ToMany:
                {
                    var domDataCollection = toApiResourceIdentifier != null
                        ? this.DomReadWriteRelationship.SetDomDataCollection(new[] { toApiResourceIdentifier }, toClrType)
                        : this.DomReadWriteRelationship.SetDomDataCollectionEmpty();
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

        public IRelationshipBuilder<TParentBuilder, TResource> SetId<TResourceId>(IEnumerable<TResourceId> clrResourceIdCollection)
        {
            // Build a JSON API resource identifier collection from the given relation name and CLR related resource identifier collection.
            var rel = this.Rel;
            var resourceType = this.ResourceType;
            var relationship = resourceType.GetRelationshipInfo(rel);

            var toClrType = relationship.ToClrType;

            var toResourceType = this.ServiceModel.GetResourceType(toClrType);
            var toApiResourceIdentifierCollection = clrResourceIdCollection
                .EmptyIfNull()
                .Select(toResourceType.CreateApiResourceIdentifier)
                .Where(toApiResourceIdentifier => toApiResourceIdentifier != null)
                .ToList();

            var toCardinality = relationship.ToCardinality;
            switch (toCardinality)
            {
                case RelationshipCardinality.ToOne:
                {
                    var clrResourceTypeName = resourceType.ClrType.Name;
                    var detail = InfrastructureErrorStrings.DocumentBuildExceptionDetailBuildToOneRelationshipResourceLinkageCardinalityMismatch
                                                           .FormatWith(clrResourceTypeName, rel);
                    throw new DocumentBuildException(detail);
                }

                case RelationshipCardinality.ToMany:
                {
                    this.DomReadWriteRelationship.SetDomDataCollection(toApiResourceIdentifierCollection, toClrType);
                }
                break;

                default:
                {
                    var detail = InfrastructureErrorStrings.InternalErrorExceptionDetailUnknownEnumerationValue
                                                           .FormatWith(typeof(RelationshipCardinality).Name, toCardinality);
                    throw new InternalErrorException(detail);
                }
            }

>>>>>>> 2cc26cdda1ac11d423461c703d9bc6adb524bb5c
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
        internal RelationshipBuilder(TParentBuilder parentBuilder, IServiceModel serviceModel, IContainerNode<DomNodeType> domContainerNode, string rel)
            : base(serviceModel, domContainerNode, rel)
        {
            Contract.Requires(parentBuilder != null);

            this.ParentBuilder = parentBuilder;
        }
        #endregion

        // PRIVATE PROPERTIES ///////////////////////////////////////////////
        #region Properties
        private TParentBuilder ParentBuilder { get; }
        #endregion
    }
}
