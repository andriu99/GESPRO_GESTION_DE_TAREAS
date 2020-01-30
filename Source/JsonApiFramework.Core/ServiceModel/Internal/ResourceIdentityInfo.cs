﻿// Copyright (c) 2015–Present Scott McDonald. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.md in the project root for license information.

using System;
using System.Diagnostics.Contracts;

using JsonApiFramework.Expressions;
using JsonApiFramework.Json;
using JsonApiFramework.Reflection;

using Newtonsoft.Json;

namespace JsonApiFramework.ServiceModel.Internal
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    internal class ResourceIdentityInfo : JsonObject
        , IResourceIdentityInfo
    {
        // PUBLIC CONSTRUCTORS //////////////////////////////////////////////
        #region Constructors
        public ResourceIdentityInfo(string apiType)
        {
            Contract.Requires(String.IsNullOrWhiteSpace(apiType) == false);

            this.ApiType = apiType;
        }

        public ResourceIdentityInfo(string apiType, IPropertyInfo id)
        {
            Contract.Requires(String.IsNullOrWhiteSpace(apiType) == false);
            Contract.Requires(id != null);

            this.ApiType = apiType;
            this.Id = id;

            this.InitializeDefaultClrId();
        }
        #endregion

        // PUBLIC PROPERTIES ////////////////////////////////////////////////
        #region IResourceIdentityInfo Implementation
        [JsonProperty] public string ApiType { get; internal set; }
        [JsonProperty] public IPropertyInfo Id { get; internal set; }
        #endregion

        // PUBLIC METHODS ///////////////////////////////////////////////////
        #region IResourceIdentityInfo Implementation
        public string GetApiId(object clrResource)
        {
            if (this.IsSingleton())
                return null;

            var clrId = this.Id.GetClrProperty(clrResource);
            var apiId = this.IsClrIdNull(clrId) == false
                ? TypeConverter.Convert<string>(clrId)
                : null;
            return apiId;
        }

        public object GetClrId(object clrResource)
        {
            if (this.IsSingleton())
                return null;

            var clrId = this.Id.GetClrProperty(clrResource);
            return clrId;
        }

        public bool IsClrIdNull(object clrId)
        {
            if (this.IsSingleton())
                return true;

            if (this.ClrIdDefaultValue == null)
            {
                return clrId == null;
            }

            return Object.Equals(this.ClrIdDefaultValue, clrId);
        }

        public void SetClrId(object clrResource, object clrId)
        {
            if (this.IsSingleton())
            {
                var idPropertyInfoMissingException = this.CreateIdPropertyInfoMissingException();
                throw idPropertyInfoMissingException;
            }

            this.Id.SetClrProperty(clrResource, clrId);
        }

        public string ToApiId(object clrId)
        {
            if (this.IsSingleton())
                return null;

            var convertedApiId = this.IsClrIdNull(clrId) == false
                ? TypeConverter.Convert<string>(clrId)
                : null;
            return convertedApiId;
        }

        public object ToClrId(object clrId)
        {
            if (this.IsSingleton())
                return null;

            var convertedClrId = TypeConverter.Convert(clrId, this.Id.ClrPropertyType);
            return convertedClrId;
        }
        #endregion

        // INTERNAL CONSTRUCTORS ////////////////////////////////////////////
        #region Constructors
        internal ResourceIdentityInfo()
        { }
        #endregion

        // PRIVATE PROPERTIES ///////////////////////////////////////////////
        #region Properties
        private object ClrIdDefaultValue { get; set; }
        #endregion

        // PRIVATE METHODS //////////////////////////////////////////////////
        #region Helper Methods
        private static Delegate CreateClrIdDefaultMethod(IPropertyInfo clrIdPropertyInfo)
        {
            Contract.Requires(clrIdPropertyInfo != null);

            var clrIdType = clrIdPropertyInfo.ClrPropertyType;
            var clrIdDefaultExpression = ExpressionBuilder.Default(clrIdType);
            var clrIdDefaultMethod = clrIdDefaultExpression.Compile();
            return clrIdDefaultMethod;
        }

        private ServiceModelException CreateIdPropertyInfoMissingException()
        {
            var resourceTypeDescription = "{0} [clrType={1}]".FormatWith(typeof(ResourceType), this.Id.ClrDeclaringType.Name);

            var idPropertyInfoDescription = "Id{0}".FormatWith(typeof(PropertyInfo).Name);

            var detail = CoreErrorStrings.ServiceModelExceptionDetailMissingMetadata
                                         .FormatWith(resourceTypeDescription, idPropertyInfoDescription);
            var idPropertyInfoMissingException = new ServiceModelException(detail);
            return idPropertyInfoMissingException;
        }

        private void InitializeDefaultClrId()
        {
            var clrIdPropertyInfo = this.Id;
            var clrIdDefaultMethod = CreateClrIdDefaultMethod(clrIdPropertyInfo);

            var clrIdDefaultValue = clrIdDefaultMethod.DynamicInvoke();
            this.ClrIdDefaultValue = clrIdDefaultValue;
        }
        #endregion
    }
}