using SimpleJSON;

public interface ISerializable 
{
    public string Name();

    public void Deserialize(JSONObject obj);

    public JSONObject Serialize();
}
