// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
// 
// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Bot.Builder.Luis.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    public partial class ActionParameter
    {
        /// <summary>
        /// Initializes a new instance of the ActionParameter class.
        /// </summary>
        public ActionParameter() { }

        /// <summary>
        /// Initializes a new instance of the ActionParameter class.
        /// </summary>
        public ActionParameter(string name = default(string), bool? required = default(bool?), IList<EntityRecommendation> value = default(IList<EntityRecommendation>))
        {
            Name = name;
            Required = required;
            Value = value;
        }

        /// <summary>
        /// Name of the parameter.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// True if the parameter is required, false otherwise.
        /// </summary>
        [JsonProperty(PropertyName = "required")]
        public bool? Required { get; set; }

        /// <summary>
        /// Value of extracted entities for this parameter.
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public IList<EntityRecommendation> Value { get; set; }

    }
}
