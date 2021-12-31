namespace PittJohnstownAPI.Models.Dining;

public class DiningLocationInfoModel
{
    public DiningLocationInfoModel(string name, bool isOpen, string message)
    {
        Name = name;
        IsOpen = isOpen;
        Message = message;
    }

    public string Name { get; set; }
    public bool IsOpen { get; set; }
    public string Message { get; set; }
}