namespace Common.Exceptions
{
    public  class FieldError : Error
    {
        public FieldError() { }
        public string PropertyName { get; set; }

    }
}
