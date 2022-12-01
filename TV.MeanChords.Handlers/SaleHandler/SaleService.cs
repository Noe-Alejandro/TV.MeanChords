using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TV.MeanChords.Data.Db.Context.DiscosChowell;
using TV.MeanChords.Data.Db.UnitOfWork;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.SaleHandler
{
    public class SaleService : ISaleService
    {
        private UoWDiscosChowell UoWDiscosChowell { get; set; }
        private List<DeliveryServiceModel> deliveryServices;
        public static SaleService Create() => new SaleService();
        public SaleService()
        {
            UoWDiscosChowell = UoWDiscosChowell.Create();
            deliveryServices = new List<DeliveryServiceModel>()
            {
                new DeliveryServiceModel(){ DeliveryId = 1, Name = "DHL"},
                new DeliveryServiceModel(){ DeliveryId = 2, Name = "UPS"},
                new DeliveryServiceModel(){ DeliveryId = 3, Name = "FEDEX"},
                new DeliveryServiceModel(){ DeliveryId = 4, Name = "EMS"}
            };
        }
        public void Dispose()
        {
            UoWDiscosChowell.Dispose();
            UoWDiscosChowell = null;
        }

        public ResponseBase<SaleResponse> PostSale(float Total, string DeliveryService, int AddressId, int UserId)
        {
            SendEmail(null);
            var sale = new Sale
            {
                Date = DateTime.Now,
                Total = Total,
                UserId = UserId,
                AddressId = AddressId,
                Status = 1,
                DeliveryService = deliveryServices.Find(x => x.Name.Equals(DeliveryService)).DeliveryId
            };
            UoWDiscosChowell.SaleRepository.Insert(sale);
            UoWDiscosChowell.Save();
            var discInShoppingCar = UoWDiscosChowell.ShoppingCarRepository.Get(x => x.UserId.Equals(UserId)).ToList();
            var saleDetails = new List<SaleDisc>();
            var discLstToReduce = new List<Disc>();
            foreach (var disc in discInShoppingCar)
            {
                saleDetails.Add(new SaleDisc
                {
                    SaleId = sale.SaleId,
                    DiscId = disc.DiscId,
                    Amount = 1,
                    Total = 1 * disc.Disc.Price
                });
                discLstToReduce.Add(disc.Disc);
            }
            UoWDiscosChowell.SaleDiscRepository.InsertByRange(saleDetails);
            UoWDiscosChowell.Save();

            var discToNotify = new List<Disc>();
            foreach (var disc in discLstToReduce)
            {
                disc.Amount -= 1;
                if (disc.Amount <= 3)
                {
                    discToNotify.Add(disc);
                }
            }
            //Send to notity => discToNotify
            UoWDiscosChowell.ShoppingCarRepository.DeleteByRange(discInShoppingCar);
            UoWDiscosChowell.Save();
            return ResponseBase<SaleResponse>.Create(new SaleResponse
            {
                SaleId = sale.SaleId
            });
        }

        private class DeliveryServiceModel
        {
            public int DeliveryId { get; set; }
            public string Name { get; set; } 
        }

        private void SendEmail(List<Disc> discs)
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("kirby.coders@gmail.com", "rdqbmxljztqbnivz");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("kirby.coders@gmail.com", "Prueba");
            mail.To.Add(new MailAddress("noe.alejandro.gonzalez14@gmail.com"));
            mail.Subject = "Mensaje de prueba";
            mail.IsBodyHtml = true;
            mail.Body = RenderBody();
            //smtp.Send(mail);
        }

        private string RenderBody()
        {
            return HTML;
        }

        private string HTML = "<body>" +
"<tbody>" +
"<tr>" +
"<td class=\"m_-1176670902229383369mpy-35 m_-1176670902229383369mpx-15\" bgcolor=\"#212429\"" +
"style=\"padding:80px\">" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td style=\"font-size:0pt;line-height:0pt;text-align:left;padding-bottom:45px\">" +
"<a href=\"https://store.steampowered.com/\" target=\"_blank\"" +
"data-saferedirecturl=\"https://www.google.com/url?q=https://store.steampowered.com/&amp;source=gmail&amp;ust=1670006113107000&amp;usg=AOvVaw2THKO81CgfFHrelk_VDzSy\">" +
"<img src=\"https://ci3.googleusercontent.com/proxy/FdwJ03uXjHFrHyCGI7WlRsrccFXjalRZcN1HFkz0xNM8NAXgxQwFrVK1NKpFelhtFEw-MHPPj3UhtsbI_UhHf_CzF1o4vphMdK1Ja3z8OxHxQkG3eW7crn8H3Miv8BQ=s0-d-e1-ft#https://store.cloudflare.steamstatic.com/public/shared/images/email/logo.png\"" +
"width=\"615\" height=\"88\" border=\"0\" alt=\"Steam\" class=\"CToWUd\" data-bit=\"iit\">" +
"</a>" +
"" +
"</td>" +
"</tr>" +
"<tr>" +
"<td>" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td" +
"style=\"font-size:36px;line-height:42px;font-family:Arial,sans-serif,'Motiva Sans';text-align:left;padding-bottom:30px;color:#bfbfbf;font-weight:bold\">" +
"Hola, {USERNAME}:</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td style=\"padding-top:10px\" colspan=\"3\"></td>" +
"</tr>" +
"<tr>" +
"<td style=\"font-family:'Motiva Sans',Arial,sans-serif;padding-top:10px;padding-bottom:20px;color:#6892a7;font-size:14px;text-transform:uppercase\"" +
"colspan=\"3\">Tus discos comprados en Discos Chowell</td>" +
"</tr>" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td style=\"padding-bottom:1px;padding-top:10px\">" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td style=\"padding-bottom:1px\">" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td bgcolor=\"#2d3239\" style=\"padding:15px\">" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<th class=\"m_-1176670902229383369column\"" +
"width=\"122\"" +
"style=\"font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal\">" +
"<table width=\"100%\" border=\"0\"" +
"cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td class=\"m_-1176670902229383369fluid-img\"" +
"style=\"font-size:0pt;line-height:0pt;text-align:left\">" +
"<img src=\"https://ci6.googleusercontent.com/proxy/lUsR9d5kj9W6NBtQ17X2O2dWJfie5rk0pkFrA2A25w74GJa5l1-rh1u0zsSsBDqyZ9QGYyMsMITXgN6SGlsuV4RooB-kxhrWcZOWKyjMCyFcV6AgaHgtYbuZwjrFBl2h9D3O52Qjr6QGpzI=s0-d-e1-ft#https://cdn.cloudflare.steamstatic.com/steam/subs/132417/capsule_231x87.jpg?t=1487788530\"" +
"width=\"122\"" +
"height=\"46\"" +
"border=\"0\"" +
"alt=\"\"" +
"class=\"CToWUd\"" +
"data-bit=\"iit\">" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</th>" +
"<th class=\"m_-1176670902229383369column m_-1176670902229383369mpb-15\"" +
"width=\"15\"" +
"style=\"font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal\">" +
"</th>" +
"<th class=\"m_-1176670902229383369column\"" +
"width=\"200\"" +
"style=\"font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal\">" +
"<table width=\"100%\" border=\"0\"" +
"cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td class=\"m_-1176670902229383369mt-left\"" +
"style=\"font-size:17px;line-height:22px;font-family:'Motiva Sans',Arial,sans-serif;text-align:left;color:#ffffff\">" +
"<strong>Resident" +
"Evil 7 -" +
"Season" +
"Pass<br></strong>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</th>" +
"<th class=\"m_-1176670902229383369column m_-1176670902229383369mpb-15\"" +
"width=\"15\"" +
"style=\"font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal\">" +
"</th>" +
"<th class=\"m_-1176670902229383369column\"" +
"style=\"vertical-align:top;font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal\">" +
"" +
"<table width=\"100%\" border=\"0\"" +
"cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"" +
"<tbody>" +
"<tr>" +
"<td class=\"m_-1176670902229383369mt-left\"" +
"style=\"font-size:11px;line-height:17px;font-family:'Motiva Sans',Arial,sans-serif;color:#f1f1f1;text-align:right;padding-bottom:5px\">" +
"Subtotal (IVA no" +
"incluido): Mex$" +
"138.39 </td>" +
"</tr>" +
"<tr>" +
"<td class=\"m_-1176670902229383369mt-left\"" +
"style=\"font-size:11px;line-height:17px;font-family:'Motiva Sans',Arial,sans-serif;color:#f1f1f1;text-align:right;padding-bottom:5px\">" +
"IVA (16&nbsp;%):" +
"Mex$ 22.14 </td>" +
"</tr>" +
"<tr>" +
"<td class=\"m_-1176670902229383369mt-left\"" +
"style=\"font-size:14px;line-height:20px;font-family:'Motiva Sans',Arial,sans-serif;color:#ffffff;text-align:right\">" +
"<strong>Total:" +
"Mex$" +
"160.53</strong>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</th>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td style=\"padding-bottom:1px\">" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td bgcolor=\"#2d3239\" style=\"padding:15px\">" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<th class=\"m_-1176670902229383369column\"" +
"width=\"122\"" +
"style=\"font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal\">" +
"<table width=\"100%\" border=\"0\"" +
"cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td class=\"m_-1176670902229383369fluid-img\"" +
"style=\"font-size:0pt;line-height:0pt;text-align:left\">" +
"<img src=\"https://ci6.googleusercontent.com/proxy/Er2yXhVwGpTCz1uwV_PgdEuf820jIXlnV5YAQLEYVpXL954nZzUCAlmyolMmorOUFZGU8fttVp1yF8RmFjf5rezzXXhb_ZemGAK-gCX0gwGkSCuCpwuB6mCGsD5rkFKweHErC4GklTXKUDI=s0-d-e1-ft#https://cdn.cloudflare.steamstatic.com/steam/apps/418370/capsule_231x87.jpg?t=1656996016\"" +
"width=\"122\"" +
"height=\"46\"" +
"border=\"0\"" +
"alt=\"\"" +
"class=\"CToWUd\"" +
"data-bit=\"iit\">" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</th>" +
"<th class=\"m_-1176670902229383369column m_-1176670902229383369mpb-15\"" +
"width=\"15\"" +
"style=\"font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal\">" +
"</th>" +
"<th class=\"m_-1176670902229383369column\"" +
"width=\"200\"" +
"style=\"font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal\">" +
"<table width=\"100%\" border=\"0\"" +
"cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td class=\"m_-1176670902229383369mt-left\"" +
"style=\"font-size:17px;line-height:22px;font-family:'Motiva Sans',Arial,sans-serif;text-align:left;color:#ffffff\">" +
"<strong>RESIDENT" +
"EVIL" +
"7<br></strong>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</th>" +
"<th class=\"m_-1176670902229383369column m_-1176670902229383369mpb-15\"" +
"width=\"15\"" +
"style=\"font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal\">" +
"</th>" +
"<th class=\"m_-1176670902229383369column\"" +
"style=\"vertical-align:top;font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal\">" +
"" +
"<table width=\"100%\" border=\"0\"" +
"cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"" +
"<tbody>" +
"<tr>" +
"<td class=\"m_-1176670902229383369mt-left\"" +
"style=\"font-size:11px;line-height:17px;font-family:'Motiva Sans',Arial,sans-serif;color:#f1f1f1;text-align:right;padding-bottom:5px\">" +
"Subtotal (IVA no" +
"incluido): Mex$" +
"134.93 </td>" +
"</tr>" +
"<tr>" +
"<td class=\"m_-1176670902229383369mt-left\"" +
"style=\"font-size:11px;line-height:17px;font-family:'Motiva Sans',Arial,sans-serif;color:#f1f1f1;text-align:right;padding-bottom:5px\">" +
"IVA (16&nbsp;%):" +
"Mex$ 21.59 </td>" +
"</tr>" +
"<tr>" +
"<td class=\"m_-1176670902229383369mt-left\"" +
"style=\"font-size:14px;line-height:20px;font-family:'Motiva Sans',Arial,sans-serif;color:#ffffff;text-align:right\">" +
"<strong>Total:" +
"Mex$" +
"156.52</strong>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</th>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"" +
"" +
"" +
"" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td style=\"padding-bottom:10px;padding-top:10px\">" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<th class=\"m_-1176670902229383369column-top\"" +
"valign=\"top\" width=\"375\" bgcolor=\"#17191c\"" +
"style=\"font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal;vertical-align:top\">" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td style=\"padding:15px\">" +
"<table width=\"100%\" border=\"0\"" +
"cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td" +
"style=\"padding-bottom:5px\">" +
"<table" +
"width=\"100%\"" +
"border=\"0\"" +
"cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td valign=\"top\"" +
"width=\"140\"" +
"style=\"font-size:12px;line-height:18px;font-family:'Motiva Sans',Arial,sans-serif;text-align:left;color:#f1f1f1\">" +
"Nombre" +
"de" +
"la" +
"cuenta:" +
"</td>" +
"<td valign=\"top\"" +
"width=\"10\"" +
"style=\"font-size:0pt;line-height:0pt;text-align:left\">" +
"</td>" +
"<td valign=\"top\"" +
"class=\"m_-1176670902229383369mt-right\"" +
"style=\"font-size:12px;line-height:18px;font-family:'Motiva Sans',Arial,sans-serif;text-align:left;color:#f1f1f1\">" +
"<strong>noe_s&ZeroWidthSpace;hi&ZeroWidthSpace;</strong>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"<tr>" +
"<td" +
"style=\"padding-bottom:5px\">" +
"<table" +
"width=\"100%\"" +
"border=\"0\"" +
"cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td valign=\"top\"" +
"width=\"140\"" +
"style=\"font-size:12px;line-height:18px;font-family:'Motiva Sans',Arial,sans-serif;text-align:left;color:#f1f1f1\">" +
"Factura:" +
"</td>" +
"<td valign=\"top\"" +
"width=\"10\"" +
"style=\"font-size:0pt;line-height:0pt;text-align:left\">" +
"</td>" +
"<td valign=\"top\"" +
"class=\"m_-1176670902229383369mt-right\"" +
"style=\"font-size:12px;line-height:18px;font-family:'Motiva Sans',Arial,sans-serif;text-align:left;color:#f1f1f1\">" +
"<strong>3230798760505566735</strong>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"<tr>" +
"<td" +
"style=\"padding-bottom:5px\">" +
"<table" +
"width=\"100%\"" +
"border=\"0\"" +
"cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td valign=\"top\"" +
"width=\"140\"" +
"style=\"font-size:12px;line-height:18px;font-family:'Motiva Sans',Arial,sans-serif;text-align:left;color:#f1f1f1\">" +
"Fecha" +
"de" +
"emisión:" +
"</td>" +
"<td valign=\"top\"" +
"width=\"10\"" +
"style=\"font-size:0pt;line-height:0pt;text-align:left\">" +
"</td>" +
"<td valign=\"top\"" +
"class=\"m_-1176670902229383369mt-right\"" +
"style=\"font-size:12px;line-height:18px;font-family:'Motiva Sans',Arial,sans-serif;text-align:left;color:#f1f1f1\">" +
"<strong>22" +
"NOV" +
"2022" +
"a" +
"las" +
"16:42" +
"CST</strong>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</th>" +
"<th class=\"m_-1176670902229383369column m_-1176670902229383369mpb-10\"" +
"width=\"10\"" +
"style=\"font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal\">" +
"</th>" +
"<th class=\"m_-1176670902229383369column-top\"" +
"valign=\"top\" bgcolor=\"#17191c\"" +
"style=\"font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal;vertical-align:top\">" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td style=\"padding:15px\">" +
"<table width=\"100%\" border=\"0\"" +
"cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td" +
"style=\"padding-bottom:5px\">" +
"<table" +
"width=\"100%\"" +
"border=\"0\"" +
"cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td valign=\"top\"" +
"style=\"font-size:12px;line-height:18px;font-family:'Motiva Sans',Arial,sans-serif;text-align:left;color:#f1f1f1\">" +
"Subtotal" +
"(IVA" +
"no" +
"incluido):" +
"</td>" +
"<td valign=\"top\"" +
"width=\"10\"" +
"style=\"font-size:0pt;line-height:0pt;text-align:left\">" +
"</td>" +
"<td valign=\"top\"" +
"style=\"font-size:12px;line-height:18px;font-family:'Motiva Sans',Arial,sans-serif;color:#f1f1f1;text-align:right\">" +
"Mex$" +
"273.32" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"<tr>" +
"<td" +
"style=\"padding-bottom:5px\">" +
"<table" +
"width=\"100%\"" +
"border=\"0\"" +
"cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td valign=\"top\"" +
"style=\"font-size:12px;line-height:18px;font-family:'Motiva Sans',Arial,sans-serif;text-align:left;color:#f1f1f1\">" +
"IVA" +
"(16&nbsp;%):" +
"</td>" +
"<td valign=\"top\"" +
"width=\"10\"" +
"style=\"font-size:0pt;line-height:0pt;text-align:left\">" +
"</td>" +
"<td valign=\"top\"" +
"style=\"font-size:12px;line-height:18px;font-family:'Motiva Sans',Arial,sans-serif;color:#f1f1f1;text-align:right\">" +
"Mex$" +
"43.73" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"<tr>" +
"<td" +
"style=\"padding-bottom:5px\">" +
"<table" +
"width=\"100%\"" +
"border=\"0\"" +
"cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td valign=\"top\"" +
"style=\"font-size:12px;line-height:18px;font-family:'Motiva Sans',Arial,sans-serif;text-align:left;color:#f1f1f1\">" +
"Total:" +
"</td>" +
"<td valign=\"top\"" +
"width=\"10\"" +
"style=\"font-size:0pt;line-height:0pt;text-align:left\">" +
"</td>" +
"<td valign=\"top\"" +
"style=\"font-size:12px;line-height:18px;font-family:'Motiva Sans',Arial,sans-serif;color:#f1f1f1;text-align:right\">" +
"Mex$" +
"317.05" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</th>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"" +
"" +
"" +
"" +
"" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td style=\"padding-bottom:10px\">" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td bgcolor=\"#17191c\" style=\"padding:15px\">" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td align=\"left\">" +
"<table border=\"0\"" +
"cellspacing=\"0\"" +
"cellpadding=\"0\"" +
"class=\"m_-1176670902229383369mw-100p\">" +
"<tbody>" +
"<tr>" +
"" +
"" +
"" +
"" +
"<th class=\"m_-1176670902229383369column-top\"" +
"valign=\"top\"" +
"style=\"font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal;vertical-align:top\">" +
"<table" +
"width=\"100%\"" +
"border=\"0\"" +
"cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td" +
"style=\"font-size:14px;line-height:20px;font-family:'Motiva Sans',Arial,sans-serif;text-align:left;color:#f1f1f1\">" +
"<strong>Dirección" +
"de" +
"facturación:</strong>" +
"<br>" +
"Noé" +
"González" +
"Bautista" +
"<br>" +
"Reg" +
"236" +
"Mz" +
"22" +
"Lt" +
"5" +
"<br>" +
"Quintana" +
"Roo," +
"77527" +
"<br>" +
"México" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</th>" +
"<th class=\"m_-1176670902229383369column m_-1176670902229383369mpb-15\"" +
"width=\"40\"" +
"style=\"font-size:0pt;line-height:0pt;padding:0;margin:0;font-weight:normal\">" +
"</th>" +
"" +
"" +
"" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"" +
"" +
"" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td style=\"padding-bottom:35px\">" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td bgcolor=\"#17191c\" style=\"padding:15px\">" +
"<table width=\"100%\" border=\"0\" cellspacing=\"0\"" +
"cellpadding=\"0\">" +
"<tbody>" +
"<tr>" +
"<td" +
"style=\"font-size:12px;line-height:18px;font-family:'Motiva Sans',Arial,sans-serif;text-align:left;color:#f1f1f1\">" +
"Valve Corporation<br>PO Box" +
"1688<br>Bellevue, WA" +
"98009<br>United States<br> Id." +
"de IVA: VCO030424EXA <br>" +
"<br>" +
"Por favor, ten en cuenta que" +
"esto no es una dirección de" +
"devolución." +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</td>" +
"</tr>" +
"</tbody>" +
"</table>" +
"</tbody>" +
"</body>";
    }
}
