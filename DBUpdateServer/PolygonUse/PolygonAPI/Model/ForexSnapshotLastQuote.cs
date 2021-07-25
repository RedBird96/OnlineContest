/* 
 * Polygon API
 *
 * The future of fintech.
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using SwaggerDateConverter = PolygonIO.Client.SwaggerDateConverter;

namespace PolygonIO.Model
{
    /// <summary>
    /// ForexSnapshotLastQuote
    /// </summary>
    [DataContract]
        public partial class ForexSnapshotLastQuote :  IEquatable<ForexSnapshotLastQuote>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForexSnapshotLastQuote" /> class.
        /// </summary>
        /// <param name="a">The ask price..</param>
        /// <param name="b">The bid price..</param>
        /// <param name="t">The Unix Msec timestamp for the start of the aggregate window..</param>
        /// <param name="x">The exchange ID on which this quote happened..</param>
        public ForexSnapshotLastQuote(double? a = default(double?), double? b = default(double?), int? t = default(int?), int? x = default(int?))
        {
            this.A = a;
            this.B = b;
            this.T = t;
            this.X = x;
        }
        
        /// <summary>
        /// The ask price.
        /// </summary>
        /// <value>The ask price.</value>
        [DataMember(Name="a", EmitDefaultValue=false)]
        public double? A { get; set; }

        /// <summary>
        /// The bid price.
        /// </summary>
        /// <value>The bid price.</value>
        [DataMember(Name="b", EmitDefaultValue=false)]
        public double? B { get; set; }

        /// <summary>
        /// The Unix Msec timestamp for the start of the aggregate window.
        /// </summary>
        /// <value>The Unix Msec timestamp for the start of the aggregate window.</value>
        [DataMember(Name="t", EmitDefaultValue=false)]
        public int? T { get; set; }

        /// <summary>
        /// The exchange ID on which this quote happened.
        /// </summary>
        /// <value>The exchange ID on which this quote happened.</value>
        [DataMember(Name="x", EmitDefaultValue=false)]
        public int? X { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ForexSnapshotLastQuote {\n");
            sb.Append("  A: ").Append(A).Append("\n");
            sb.Append("  B: ").Append(B).Append("\n");
            sb.Append("  T: ").Append(T).Append("\n");
            sb.Append("  X: ").Append(X).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as ForexSnapshotLastQuote);
        }

        /// <summary>
        /// Returns true if ForexSnapshotLastQuote instances are equal
        /// </summary>
        /// <param name="input">Instance of ForexSnapshotLastQuote to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ForexSnapshotLastQuote input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.A == input.A ||
                    (this.A != null &&
                    this.A.Equals(input.A))
                ) && 
                (
                    this.B == input.B ||
                    (this.B != null &&
                    this.B.Equals(input.B))
                ) && 
                (
                    this.T == input.T ||
                    (this.T != null &&
                    this.T.Equals(input.T))
                ) && 
                (
                    this.X == input.X ||
                    (this.X != null &&
                    this.X.Equals(input.X))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.A != null)
                    hashCode = hashCode * 59 + this.A.GetHashCode();
                if (this.B != null)
                    hashCode = hashCode * 59 + this.B.GetHashCode();
                if (this.T != null)
                    hashCode = hashCode * 59 + this.T.GetHashCode();
                if (this.X != null)
                    hashCode = hashCode * 59 + this.X.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}