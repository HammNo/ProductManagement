namespace ProductManagement.ASP.Exceptions
{
    public class ModelException : Exception
    {
        public ModelException(string field, string toPrint) : base(toPrint)
        {
            Field = field;
        }
        public string Field { get; set; }
    }
}
