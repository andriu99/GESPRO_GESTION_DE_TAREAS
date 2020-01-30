// Copyright (c) 2015–Present Scott McDonald. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.md in the project root for license information.

using System;
using System.Diagnostics.Contracts;
using System.Linq;

using JsonApiFramework.Internal.Tree;
using JsonApiFramework.ServiceModel;

namespace JsonApiFramework.Internal.Dom
{
    internal static class ResourceTypeExtensions
    {
        // PUBLIC METHODS ///////////////////////////////////////////////////
        #region Extensions Methods
        public static void MapClrTypeToDomResource(this IResourceType resourceType, DomReadWriteResource domResource)
        {
            Contract.Requires(resourceType != null);
            Contract.Requires(domResource != null);

            domResource.CreateAndAddNode(() => DomType.CreateFromResourceType(resourceType));
        }

        public static void MapClrIdToDomResource(this IResourceType resourceType, DomReadWriteResource domResource, object clrResource)
        {
            Contract.Requires(resourceType != null);
            Contract.Requires(domResource != null);

            if (clrResource == null)
                return;

            domResource.CreateAndAddNode(() => DomId.CreateFromClrResource(resourceType, clrResource));
        }

        public static void MapClrAttributesToDomResource(this IResourceType resourceType, DomReadWriteResource domResource, object clrResource)
        {
            Contract.Requires(resourceType != null);
            Contract.Requires(domResource != null);

            if (clrResource == null)
                return;

            var domAttributes = domResource.CreateAndAddNode(() => DomAttributes.Create());

            var attributeCollection = resourceType.Attributes.Collection;
            foreach (var attribute in attributeCollection)
            {
                var localAttribute = attribute;
                domAttributes.CreateAndAddNode(() => DomAttribute.CreateFromClrResource(localAttribute, clrResource));
            }
        }

        public static void MapClrAttributeToDomAttributes(this IResourceType resourceType, DomAttributes domAttributes, string clrAttributeName, object clrAttribute)
        {
            Contract.Requires(resourceType != null);
            Contract.Requires(domAttributes != null);
            Contract.Requires(String.IsNullOrWhiteSpace(clrAttributeName) == false);

            var attribute = resourceType.GetClrAttribute(clrAttributeName);
            domAttributes.CreateAndAddNode(() => DomAttribute.CreateFromClrAttribute(attribute, clrAttribute));
        }

        public static void MapDomResourceToClrMeta(this IResourceType resourceType, object clrResource, DomReadWriteResource domResource)
        {
            Contract.Requires(resourceType != null);
            Contract.Requires(clrResource != null);
            Contract.Requires(domResource != null);

            var domMetaNode = domResource.GetNode(DomNodeType.Meta);
            if (domMetaNode == null)
                return;

            var domMeta = (IDomMeta)domMetaNode;
            var clrMeta = domMeta.Meta;
            resourceType.SetClrMeta(clrResource, clrMeta);
        }

        public static void MapDomResourceToClrId(this IResourceType resourceType, object clrResource, DomReadWriteResource domResource)
        {
            Contract.Requires(resourceType != null);
            Contract.Requires(clrResource != null);
            Contract.Requires(domResource != null);

            var domIdNode = domResource.GetNode<DomNodeType, DomId>(DomNodeType.Id);
            if (domIdNode == null)
                return;

            var clrId = domIdNode.ClrId;
            resourceType.SetClrId(clrResource, clrId);
        }

        public static void MapDomResourceToClrAttributes(this IResourceType resourceType, object clrResource, DomReadWriteResource domResource)
        {
            Contract.Requires(resourceType != null);
            Contract.Requires(clrResource != null);
            Contract.Requires(domResource != null);

            var domAttributesNode = domResource.GetNode<DomNodeType, DomAttributes>(DomNodeType.Attributes);
            if (domAttributesNode == null)
                return;

            var domAttributeNodes = domAttributesNode.Nodes()
                                                     .Cast<DomAttribute>()
                                                     .ToList();
            foreach (var domAttributeNode in domAttributeNodes)
            {
                var clrPropertyName = domAttributeNode.ClrPropertyName;
                var clrPropertyValue = domAttributeNode.ClrAttribute;

                var clrAttribute = resourceType.GetClrAttribute(clrPropertyName);
                clrAttribute.SetClrProperty(clrResource, clrPropertyValue);
            }
        }

        public static void MapDomResourceToClrRelationships(this IResourceType resourceType, object clrResource, DomReadWriteResource domResource)
        {
            Contract.Requires(resourceType != null);
            Contract.Requires(clrResource != null);
            Contract.Requires(domResource != null);

            var domRelationshipsNode = domResource.GetNode(DomNodeType.Relationships);
            if (domRelationshipsNode == null)
                return;

            var domRelationships = (IDomRelationships)domRelationshipsNode;
            var clrRelationships = domRelationships.Relationships;
            resourceType.SetClrRelationships(clrResource, clrRelationships);
        }

        public static void MapDomResourceToClrLinks(this IResourceType resourceType, object clrResource, DomReadWriteResource domResource)
        {
            Contract.Requires(resourceType != null);
            Contract.Requires(clrResource != null);
            Contract.Requires(domResource != null);

            var domLinksNode = domResource.GetNode(DomNodeType.Links);
            if (domLinksNode == null)
                return;

            var domLinks = (IDomLinks)domLinksNode;
            var clrLinks = domLinks.Links;
            resourceType.SetClrLinks(clrResource, clrLinks);
        }
        #endregion
    }
}
