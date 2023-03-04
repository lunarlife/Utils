namespace Utils;

public interface ILogger
{
    public void Info(object info);
    public void Warning(object warning);
    public void Error(object error);
    public void Error(Exception e);

}