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
    
    public partial class Kunder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kunder()
        {
            this.Tilmeldingers = new HashSet<Tilmeldinger>();
        }
    
        public int Id { get; set; }
        public string Navn { get; set; }
        public string Adresse { get; set; }
        public string Telefon { get; set; }
        public string RejsearrangementId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tilmeldinger> Tilmeldingers { get; set; }
    }
}
