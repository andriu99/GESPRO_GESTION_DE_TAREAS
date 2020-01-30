﻿// Copyright (c) 2015–Present Scott McDonald. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.md in the project root for license information.

using JsonApiFramework.Json;

using Newtonsoft.Json;

namespace JsonApiFramework.TestData.ClrResources
{
    [JsonObject(MemberSerialization.OptIn)]
    public class StoreAttributes : JsonObject
    {
        [JsonProperty("store-name")] public string StoreName { get; set; }
        [JsonProperty("address")] public string Address { get; set; }
        [JsonProperty("city")] public string City { get; set; }
        [JsonProperty("state")] public string State { get; set; }
        [JsonProperty("zip-code")] public string ZipCode { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Store : JsonObject, IResource
    {
        [JsonProperty] public long StoreId { get; set; }
        [JsonProperty] public string StoreName { get; set; }
        [JsonProperty] public string Address { get; set; }
        [JsonProperty] public string City { get; set; }
        [JsonProperty] public string State { get; set; }
        [JsonProperty] public string ZipCode { get; set; }
    }
}