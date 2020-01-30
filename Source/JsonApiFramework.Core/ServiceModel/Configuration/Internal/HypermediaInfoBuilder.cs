﻿// Copyright (c) 2015–Present Scott McDonald. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using JsonApiFramework.ServiceModel.Conventions;
using JsonApiFramework.ServiceModel.Internal;

namespace JsonApiFramework.ServiceModel.Configuration.Internal
{
    internal class HypermediaInfoBuilder : IHypermediaInfoBuilder
    {
        // PUBLIC CONSTRUCTORS //////////////////////////////////////////////
        #region Constructors
        public HypermediaInfoBuilder(Type clrResourceType)
        {
            Contract.Requires(clrResourceType != null);

            var hypermediaInfoFactory = CreateHypermediaInfoFactory(clrResourceType);
            this.HypermediaInfoFactory = hypermediaInfoFactory;
        }
        #endregion

        // PUBLIC METHODS ///////////////////////////////////////////////////
        #region IHypermediaInfoBuilder Implementation
        public IHypermediaInfoBuilder SetApiCollectionPathSegment(string apiCollectionPathSegment)
        {
            this.HypermediaInfoModifierCollection = this.HypermediaInfoModifierCollection ?? new List<Action<HypermediaInfo>>();
            this.HypermediaInfoModifierCollection.Add(x => { x.ApiCollectionPathSegment = apiCollectionPathSegment; });
            return this;
        }
        #endregion

        // INTERNAL METHODS /////////////////////////////////////////////////
        #region Factory Methods
        internal IHypermediaInfo CreateHypermediaInfo(ConventionSet conventionSet)
        {
            var hypermediaInfo = this.HypermediaInfoFactory(conventionSet);

            if (this.HypermediaInfoModifierCollection == null)
                return hypermediaInfo;

            foreach (var hypermediaInfoModifier in this.HypermediaInfoModifierCollection)
            {
                hypermediaInfoModifier(hypermediaInfo);
            }

            return hypermediaInfo;
        }
        #endregion

        // PRIVATE PROPERTIES ///////////////////////////////////////////////
        #region Properties
        private Func<ConventionSet, HypermediaInfo> HypermediaInfoFactory { get; set; }
        private IList<Action<HypermediaInfo>> HypermediaInfoModifierCollection { get; set; }
        #endregion

        // PRIVATE METHODS //////////////////////////////////////////////////
        #region Methods
        private static Func<ConventionSet, HypermediaInfo> CreateHypermediaInfoFactory(Type clrResourceType)
        {
            Contract.Requires(clrResourceType != null);

            Func<ConventionSet, HypermediaInfo> hypermediaInfoFactory = (conventionSet) =>
                {
                    var apiCollectionPathSegment = clrResourceType.Name;
                    if (conventionSet != null && conventionSet.ApiTypeNamingConventions != null)
                    {
                        apiCollectionPathSegment = conventionSet.ApiTypeNamingConventions.Aggregate(apiCollectionPathSegment, (current, namingConvention) => namingConvention.Apply(current));
                    }

                    var hypermediaInfo = new HypermediaInfo
                        {
                            // HypermediaInfo Properties
                            ApiCollectionPathSegment = apiCollectionPathSegment
                        };
                    return hypermediaInfo;
                };
            return hypermediaInfoFactory;
        }
        #endregion
    }
}