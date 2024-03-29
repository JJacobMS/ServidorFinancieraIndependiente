//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatosFinancieraIndependiente
{
    using System;
    using System.Collections.Generic;
    
    public partial class Credito
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Credito()
        {
            this.Cobro = new HashSet<Cobro>();
            this.Dictamen = new HashSet<Dictamen>();
        }
    
        public int folioCredito { get; set; }
        public double monto { get; set; }
        public double saldoPendiente { get; set; }
        public double deudaExtra { get; set; }
        public System.DateTime fechaSolicitud { get; set; }
        public int CondicionCredito_idCondicionCredito { get; set; }
        public int Cliente_idCliente { get; set; }
        public int EstatusCredito_idEstatusCredito { get; set; }
        public int Checklist_idChecklist { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cobro> Cobro { get; set; }
        public virtual CondicionCredito CondicionCredito { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dictamen> Dictamen { get; set; }
        public virtual EstatusCredito EstatusCredito { get; set; }
        public virtual Checklist Checklist { get; set; }
    }
}
