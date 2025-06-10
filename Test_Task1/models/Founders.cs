using System.ComponentModel.DataAnnotations.Schema;

namespace test_task.models
{
    public class Founders
    {
        public int Id { get; set; }               // Уникальный идентификатор учредителя
        public string INN { get; set; }            // ИНН учредителя
        public string FullName { get; set; }       // ФИО учредителя
        public DateTime CreatedAt { get; set; }    // Дата добавления
        public DateTime UpdatedAt { get; set; }    // Дата обновления

        public List<Clients> Clients { get; set; }      // Список учредителей (только для ЮЛ)



    }
}
