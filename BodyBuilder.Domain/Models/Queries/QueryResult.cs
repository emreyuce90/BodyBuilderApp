using System.Collections.Generic;

namespace Flowner.Domain.Models.Queries {
    /// <summary>
    /// Sayfalanmış veriler için bir liste ve toplam kayıt sayısı belirtir.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryResult<T> {
        /// <summary>
        /// Veri listesi
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();
        /// <summary>
        /// Verinin toplam filtrelenmiş kayıt sayısı. 
        /// </summary>
        public int TotalItems { get; set; } = 0;
    }
}
