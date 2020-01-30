﻿// Copyright (c) 2015–Present Scott McDonald. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.md in the project root for license information.
namespace JsonApiFramework.JsonApi
{
    /// <summary>Abstracts any object that has a gettable <c>JsonApiVersion</c> property.</summary>
    public interface IGetJsonApiVersion
    {
        // PUBLIC PROPERTIES ////////////////////////////////////////////////
        #region Properties
        JsonApiVersion JsonApiVersion { get; }
        #endregion
    }
}
