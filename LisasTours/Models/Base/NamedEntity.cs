namespace LisasTours.Models.Base
{
    /// <summary>Именованная сущность</summary>
    public abstract class NamedEntity : Entity
    {
        /// <summary>Имя сущности</summary>
        public string Name { get; set; }
    }
}
