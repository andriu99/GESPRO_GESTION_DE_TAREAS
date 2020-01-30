﻿// Copyright (c) 2015–Present Scott McDonald. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.md in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace JsonApiFramework.Server
{
    public interface IToManyIncludedResourcesCollectionSource<TFromResource, TToResource>
        where TFromResource : class, IResource
        where TToResource : class, IResource
    {
        // PUBLIC METHODS ///////////////////////////////////////////////////
        #region Properties
        IEnumerable<IToManyIncludedResources<TFromResource, TToResource>> GetToManyIncludedResourcesCollection();

        Task<IEnumerable<IToManyIncludedResources<TFromResource, TToResource>>> GetToManyIncludedResourcesCollectionAsync();
        #endregion
    }
}
