//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VikingRejser
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rejsearrangementer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rejsearrangementer()
        {
            this.Tilmeldingers = new HashSet<Tilmeldinger>();
        }
    
        public int Id { get; set; }
        public string Titel { get; set; }
        public string By { get; set; }
        public System.DateTime Startdato { get; set; }
        public System.DateTime Slutdato { get; set; }
        public int Pris { get; set; }
        public Nullable<int> MaxAntal { get; set; }
        public string Beskrivelse { get; set; }
        public Nullable<int> Transportør { get; set; }
    
        public virtual Transportører Transportører { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tilmeldinger> Tilmeldingers { get; set; }
    }
}
