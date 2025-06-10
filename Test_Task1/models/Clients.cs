using System.Diagnostics.Metrics;

namespace test_task.models
{
    public class Clients
    {
        public int Id { get; set; }                     // Уникальный идентификатор клиента
        public string INN { get; set; }                  // ИНН клиента
        public string Name { get; set; }                 // Наименование клиента
        public string Type { get; set; }                 // Тип клиента 
        
        public DateTime CreatedAt { get; set; }          // Дата добавления
        public DateTime UpdatedAt { get; set; }          // Дата обновления

        public List<Founders> Founders { get; set; }      // Список учредителей (только для ЮЛ)
       
    }
}
