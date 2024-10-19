namespace metricas.dto;

public class HtmlIndexHeaderItem(string percentage, string title, string fraction)
{ 
    public string percentage { get; set; } = percentage;
    public string title { get; set; } = title;
    public string fraction { get; set; } = fraction;
}