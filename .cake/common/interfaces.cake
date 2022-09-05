///////////////////////////////////////////////////////////////////////////////
// INTERFACES
///////////////////////////////////////////////////////////////////////////////

public interface IKeyValuePair
{
   // properties
   int Index { get; }
   string Key { get; }
   string Value { get; }
   // methods
   bool IsValid();
   void Update(string value, bool useQuotes=false);
}

public interface ITask
{
   string[] Commands { get; set; }
}
