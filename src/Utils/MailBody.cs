using Microsoft.Extensions.Primitives;
using System.Text;
// ReSharper disable MemberCanBePrivate.Global

namespace Webamoki.Utils;
public class MailBody
{
    private readonly StringBuilder _htmlBody = new();
    private readonly string _title;
    private string? _unsubscribeText;

    public MailBody(string title)
    {
        _title = title;
        AddTitle(title);
    }

    public void AddUnsubscribeLink(string link)
    {
        var linkText = $"<a href=\"{link}\" target=\"_blank\" style=\"color: #000;\">unsubscribe</a>";
        _unsubscribeText = $"If you do not want to receive any emails like this in the future, please {linkText}.";
    }

    public void AddTitle(string title)
    {
        _ = _htmlBody.Append($"""
                          <table style="width: 600px;" width="600" border="0" cellpadding="0" cellspacing="0"  class="full-width">
                              <tr>
                                  <td style="text-align: center;background-color: #fff;padding: 20px 20px;padding-top: 10px;" class="full-width">
                                      <h1 style="margin: 0;font-size: 36px;color: #000000;">
                                          {title}
                                      </h1>
                                  </td>
                              </tr>
                          </table>
                          """);
    }

    public void AddContent(string content)
    {
        _ = _htmlBody.Append($"""
                          <table style="width: 600px;" width="600" border="0" cellpadding="0" cellspacing="0"  class="full-width">
                              <tr>
                                  <td style="text-align: center;background-color: #fff;padding: 20px 20px;padding-top: 0px;" class="full-width">
                                      {content}
                                  </td>
                              </tr>
                          </table>
                          """);
    }

    public void AddText(string text)
    {
        _ = _htmlBody.Append($"""
                          <table style="width: 600px;" width="600" border="0" cellpadding="0" cellspacing="0"  class="full-width">
                              <tr>
                                  <td style="text-align: left;background-color: #fff;padding: 0 20px 20px;" class="full-width">
                                      <p style="margin: 0;font-size: 16px;line-height: 24px;color: #000000;">
                                          {text}
                                      </p>
                                  </td>
                              </tr>
                          </table>
                          """);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="link">Full url including base</param>
    /// <param name="text"></param>
    /// <param name="colorValue">Provided as a color hexcode value including #</param>
    public void AddButton(string link, string text, string colorValue)
    {
        _ = _htmlBody.Append($"""
                         <table style="width: 600px;" width="600" border="0" cellpadding="0" cellspacing="0" class="full-width">
                             <tbody>
                                 <tr>
                                     <td style="text-align: center;background-color: #fff;padding: 20px 20px;padding-top: 15px;width: 600px;padding-bottom: 30px;box-sizing: border-box;overflow: hidden;overflow: hidden !important;padding-left: 20px;padding-right: 20px;" width="600" class="full-width">    
                                     <table style="width: 560px;" width="560" border="0" cellpadding="0" cellspacing="0" class="full-width">
                                         <tbody>
                                             <tr>
                                                 <td style="width: 560px;" width="560" align="center">
                                                     <table style="margin:auto;" border="0" cellpadding="0" cellspacing="0">
                                                         <tbody>
                                                             <tr>
                                                                 <td style="width: 125px;height: 38px;padding: 0px 10px;text-align: center;background-color: {colorValue};font-size: 16px;line-height: 0px !important;color: #fff;" width="125" height="38">
                                                                     <a href="{link}" target="_blank" style="text-decoration: none;color: #fff;text-transform: uppercase;font-weight: 500;font-size: 16px;color: #fff !important;line-height: 0px;"><span style="color: #fff;color: #fff !important;">{text}</span></a>
                                                                 </td>
                                                             </tr>
                                                         </tbody>
                                                     </table>
                                                 </td>
                                             </tr>
                                         </tbody>
                                         </table>
                                     </td>
                                 </tr>
                             </tbody>
                         </table>
                         """);
    }

    public void AddImage(string imageSrc)
    {
        _ = _htmlBody.Append($"""
                          <table style="width: 600px;" width="600" border="0" cellpadding="0" cellspacing="0"  class="full-width">
                              <tr>
                                  <td style="text-align: center;background-color: #fff;padding: 20px 0px;padding-top: 30px;" class="full-width">
                                      <img src="{imageSrc}" alt="" style="width: 600px;" width="600">
                                  </td>
                              </tr>
                          </table>
                          """);
    }

    // TODO: Move to Products Package
    public void AddOrderLine(string name, string link, string imageSrc, string price, int quantity)
    {
        _ = _htmlBody.Append($"""
                          <table style="width: 600px;" width="600" border="0" cellpadding="0" cellspacing="0"  class="full-width">
                              <tr>
                                  <td style="text-align: center;background-color: #fff;padding: 20px 20px;padding-top: 30px;padding-bottom: 0px;" class="full-width">
                                      <table style="width: 560px;" width="560" border="0" cellpadding="0" cellspacing="0"  class="full-width">
                                          <tr>
                                              <td style="width: 200px;" width="200">
                                                  <img src="{imageSrc}" alt="" style="width: 200px;" width="200">
                                              </td>
                                              <td style="width: 260px;text-align: left;padding-left: 30px;" width="260">
                                                  <h4 style="margin: 0;color: #000;font-size: 17px;"><a href="{link}" target="_blank" style="color: #000;">{name}</a></h4>
                                                  <table style="width: 230px;text-align: left;" width="230" border="0" cellpadding="0" cellspacing="0">
                                                      <tr>
                                                          <td style="padding-top: 5px;">
                                                              <p style="margin: 0;color: #000;font-size: 14px;"><b>Quantity:</b>&nbsp;&nbsp; {quantity}</p>
                                                          </td>
                                                      </tr>
                                                  </table>
                                              </td>
                                              <td style="width: 100px;text-align: right;padding-left: 20px;" width="">
                                                  <h4 style="margin: 0;color: #000;font-size: 17px;">{price}</h4>
                                              </td>
                                          </tr>
                                      </table>
                                  </td>
                              </tr>
                          </table>
                          """);
    }

    public void AddTwoColumns(string column1Title, StringValues column1Text, string column2Title,
        StringValues column2Text)
    {
        var column1Content = "";
        var column1Enumator = column1Text.GetEnumerator();
        while (column1Enumator.MoveNext()) column1Content += $"<p style=\"margin: 0;color: #000;font-size: 14px;\">{column1Enumator.Current}</p>";

        column1Enumator.Dispose();

        var column2Content = "";
        var column2Enumator = column2Text.GetEnumerator();
        while (column2Enumator.MoveNext()) column2Content += $"<p style=\"margin: 0;color: #000;font-size: 14px;\">{column2Enumator.Current}</p>";

        column2Enumator.Dispose();

        _ = _htmlBody.Append($"""
                          <table style="width: 600px;" width="600" border="0" cellpadding="0" cellspacing="0"  class="full-width">
                              <tr>
                                  <td style="text-align: center;background-color: #fff;padding: 20px 20px" class="full-width">
                                      <table style="width: 560px;" width="560" border="0" cellpadding="0" cellspacing="0"  class="full-width">
                                          <tr>
                                              <td style="vertical-align:top;width: 280px;text-align: left;padding-left: 30px;" width="280">
                                                  <h4 style="margin: 0;color: #000;font-size: 17px;">{column1Title}</h4>
                                                  <table style="width: 250px;text-align: left;" width="250" border="0" cellpadding="0" cellspacing="0">
                                                      <tr>
                                                          <td style="padding-top: 5px;">
                                                              {column1Content}
                                                          </td>
                                                      </tr>
                                                  </table>
                                              </td>
                                              <td style="vertical-align:top;width: 280px;text-align: left;padding-left: 30px;" width="280">
                                                  <h4 style="margin: 0;color: #000;font-size: 17px;">{column2Title}</h4>
                                                  <table style="width: 250px;text-align: left;" width="250" border="0" cellpadding="0" cellspacing="0">
                                                      <tr>
                                                          <td style="padding-top: 5px;">
                                                              {column2Content}
                                                          </td>
                                                      </tr>
                                                  </table>
                                              </td>
                                          </tr>
                                      </table>
                                  </td>
                              </tr>
                          </table>
                          """);
    }

    // TODO: Move to Products Package
    public void AddOrderPrices(List<(string name, string price)> prices)
    {
        var index = 0;
        foreach (var (name, price) in prices)
        {
            index++;
            var border = index == 1 ? "border-top: 2px solid #e5e5e5;" : "";
            if (index == prices.Count) border = "border-bottom: 2px solid #e5e5e5;padding-bottom:15px;";
            var padding = index == prices.Count ? "0 20px 20px 20px" : "0 20px";

            _ = _htmlBody.Append($"""
                              <table style="width: 600px;" width="600" border="0" cellpadding="0" cellspacing="0"  class="full-width">
                                  <tr>
                                      <td style="text-align: center;background-color: #fff;padding: {padding}" class="full-width">
                                          <table style="width: 560px;" width="560" border="0" cellpadding="0" cellspacing="0"  class="full-width">
                                              <tr>
                                                  <td style="{border} text-align: right;padding-top: 15px;">
                                                      <p style="margin: 0;color: #000;font-size: 22px;"><b>{name}:</b> &nbsp;&nbsp; {price}</p>
                                                  </td>
                                              </tr>
                                          </table>
                                      </td>
                                  </tr>
                              </table>
                              """);
        }
    }

    private string GetSocialsSection() =>
        // TODO: Implement this after BrandIdentity::SOCIALS is migrated
        "";

    public string GetHTMLBody(string companyName,
        string contactPhone,
        string contactEmail,
        string colorValue,
        string companyLogoUrl,
        string baseUrl)
    {
        var year = DateTime.Now.ToString("yyyy");
        var extendedLogoUrl = $"{baseUrl}/{companyLogoUrl}?v={year}{_title}";
        var noticeText = _unsubscribeText ?? "If you were not expecting to see this email, please contact us.";
        var htmlBody = _htmlBody.ToString();
        var socialsString = GetSocialsSection();

        return $$"""
                 <!doctype html>
                 <html lang="en">
                 <head>
                 <meta charset="UTF-8">
                 <meta name="viewport" content="width=device-width, initial-scale=1">
                 <meta http-equiv="X-UA-Compatible" content="IE=edge">
                 <title>{{_title}}</title>
                 <link rel="preconnect" href="https://fonts.googleapis.com">
                 <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
                 <link href="https://fonts.googleapis.com/css2?family=Roboto:ital,wght@0,100;0,300;0,400;0,500;0,700;0,900;1,100;1,300;1,400;1,500;1,700;1,900&family=Stylish&display=swap" rel="stylesheet">

                 <style>
                     *{
                         
                         box-sizing: border-box;
                         padding: 0;
                         border-spacing: 0px;
                         margin: 0;
                         padding: 0;
                         font-family: 'Roboto', sans-serif;
                     }
                     p{
                         font-family: 'Roboto', sans-serif;
                         margin: 0;
                         margin-top: 10px;
                         font-size: 16px;
                         line-height: 24px;
                         color: #000000;
                         font-weight: 400;
                         
                     }
                     h1,h2,h3,h4,h5,h6{
                         font-family: 'Roboto', sans-serif;
                         color: #000000;
                         
                     }
                     @media only screen and (max-width:450px){
                         
                         .full-width{
                             width: 100% !important;
                         }
                         br{
                             display: none !important;
                         }
                         .two-col{
                             width: 100% !important;
                             display: block !important;
                             padding-left: 0 !important;
                             padding-right: 0 !important;
                         }
                         .two-col img{
                             width: 100% !important;
                             margin-bottom: 15px !important;
                         }
                         .pl25{
                             padding-left: 25px !important;
                         }
                     }
                 </style>
                 </head>

                 <body style="margin:0;background-color: #F5F5F5;">
                 <table style="width: 100%;background-color: {{colorValue}};" cellpadding="0" cellspacing="0" border="0" align="center">
                     <tr>
                         <td align="center" style="background-color: {{colorValue}};width: 100%;" width="100%">
                 
                             
                 
                             <center>
                 
                                 <table style="border: none;padding: 0;margin:auto;" border="0" cellpadding="0" cellspacing="0" class="w100">
                                     <tr>
                                         <td style="max-width: 600px; line-height: 1.4; color: #000000;padding: 0;border-spacing: 0;border-spacing: 0;padding: 0;margin: 0;box-sizing: border-box;background-color: {{colorValue}};vertical-align: top;width: 600px;padding-top: 40px;" width="600" class="full-width">
                 
                                             
                                             
                                             <!-- Header -->
                                             <table style="width: 600px;" width="600" border="0" cellpadding="0" cellspacing="0"  class="full-width">
                                                 <tr>
                                                     <td style="text-align: center;background-color: #fff;padding: 20px 20px;" class="full-width">
                                                         <table style="width: 560px;" width="560" border="0" cellpadding="0" cellspacing="0">
                                                             <tr>
                                                                 <td style="width: 280px;text-align: left;" width="280">
                                                                     <!-- Logo Image -->
                                                                     <a href="{{baseUrl}}" target="_blank"><img src="{{extendedLogoUrl}}" alt="" style="width: 190px;" width="190"></a>
                                                                 </td>
                                                                 <td style="width: 280px;" width="280" align="right">
                                                                     <!-- Social Images -->
                                                                     <table style="">
                                                                         <tr>
                                                                             {{socialsString}}
                                                                         </tr>
                                                                     </table>
                                                                     <!--End Social Images -->
                                                                 </td>
                                                             </tr>
                                                         </table>
                                                     </td>
                                                 </tr>
                                             </table>
                                             <!-- End Header -->
                 
                 
                                         </td>
                                     </tr>
                                 </table>
                 
                 
                             </center>
                 
                 
                         </td>
                     </tr>
                 </table>
                 <table style="width: 100%;background-color: #f5f5f5;" cellpadding="0" cellspacing="0" border="0" align="center">
                     <tr>
                         <td align="center" style="background-color: #f5f5f5;width: 100%;" width="100%">
                 
                             
                 
                             <center>
                 
                                 <table style="border: none;padding: 0;margin:auto;" border="0" cellpadding="0" cellspacing="0" class="w100">
                                     <tr>
                                         <td style="max-width: 600px; line-height: 1.4; color: #000000;padding: 0;border-spacing: 0;border-spacing: 0;padding: 0;margin: 0;box-sizing: border-box;background-color: #fff;vertical-align: top;width: 600px;padding-top: 0px;" width="600" class="full-width">
                                             {{htmlBody}}
                                             <table style="width: 600px;" width="600" border="0" cellpadding="0" cellspacing="0"  class="full-width">
                                                 <tr>
                                                     <td style="text-align: center;background-color: #f5f5f5;padding: 30px 0px;padding-top: 40px;" class="full-width">
                 
                 
                                                         <!-- Support Box -->
                                                         <table style="width: 600px;" width="600" border="0" cellpadding="0" cellspacing="0"  class="full-width">
                                                             <tr>
                                                                 <td style="text-align: center;background-color: #FFE0DE;padding: 25px;padding-top: 20px;" class="full-width">
                                                                     <h3 style="margin: 0;color: #000;font-size: 22px;line-height: 35px;">Need more help? Contact us</h3>
                                                                     <p style="margin: 0;color: #000;font-size: 16px;line-height: 24px;">Mobile Support: {{contactPhone}}</p>
                                                                     <p style="margin: 0;color: #000;font-size: 16px;line-height: 24px;">Email Support: {{contactEmail}}</p>
                                                                 </td>
                                                             </tr>
                                                         </table>
                                                         <!-- End Support Box -->
                 
                                                         <!-- Notice -->
                                                         <table style="width: 600px;" width="600" border="0" cellpadding="0" cellspacing="0"  class="full-width">
                                                             <tr>
                                                                 <td style="text-align: left;background-color: #f5f5f5;padding: 25px;" class="full-width">
                                                                     {{noticeText}}
                                                                 </td>
                                                             </tr>
                                                         </table>
                                                         <!-- End Notice -->
                 
                 
                                                         <!-- Copyright -->
                                                         <table style="width: 600px;" width="600" border="0" cellpadding="0" cellspacing="0"  class="full-width">
                                                             <tr>
                                                                 <td style="text-align: left;background-color: #f5f5f5;padding: 25px;" class="full-width">
                                                                     
                                                                     <table style="width: 560px;" width="560" border="0" cellpadding="0" cellspacing="0"  class="full-width">
                                                                         <tr>
                                                                             <td style="text-align: left;">
                                                                                 <p style="margin: 0;color: #000;font-size: 14px;line-height: 22px;">
                                                                                     © Copyright {{DateTime.Now:yyyy}} {{companyName}} - All Rights Reserved
                                                                                 </p>
                                                                             </td>
                                                                             <td style="text-align: right;padding-left: 30px;">
                                                                                 <p style="margin: 0;color: #000;font-size: 14px;line-height: 22px;">Powered by <a href="https://www.webamoki.co.uk" target="_blank" style="color: #000;">Webamoki</a></p>
                                                                             </td>
                                                                         </tr>
                                                                     </table>
                                                                 </td>
                                                             </tr>
                                                         </table>
                                                         <!-- End Copyright -->
                 
                 
                                                     </td>
                                                 </tr>
                                             </table>
                                         </td>
                                     </tr>
                                 </table>
                             </center>
                         </td>
                     </tr>
                 </table>
                 </body>
                 </html>
                 """;
    }
}
