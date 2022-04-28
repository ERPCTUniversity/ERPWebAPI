using ct.Models;
using System;
using System.Collections.Generic;

namespace ct.Models
{
      public partial class WeatherInfoMaster
        {
            public int Id { get; set; }
            public string Main { get; set; }
            public string Description { get; set; }
            public Nullable<int> WeatherId { get; set; }

            public virtual WeatherInfo WeatherInfo { get; set; }
        
    }

    public class WeatherInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WeatherInfo()
        {
            this.WeatherInfoMasters = new HashSet<WeatherInfoMaster>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string temp { get; set; }
        public string Pressure { get; set; }
        public string Humidity { get; set; }
        public string Speed { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WeatherInfoMaster> WeatherInfoMasters { get; set; }
    }
}