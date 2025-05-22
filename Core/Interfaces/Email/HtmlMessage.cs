using Core.Entities;

namespace Core.Interfaces.Email;

public static class HtmlMessage
{
  public static string GetVerificationEmailTemplate(string name, string code)
  {
    return $$"""
               <!DOCTYPE html>
               <html>
               <head>
                 <meta charset="utf-8">
                 <meta name="viewport" content="width=device-width, initial-scale=1.0">
                 <title>Verificación de correo electrónico</title>
                 <style>
                   /* Estilos generales */
                   body {
                     font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                     line-height: 1.6;
                     color: #333333;
                     margin: 0;
                     padding: 0;
                     background-color: #f9f9f9;
                   }
                   .container {
                     max-width: 600px;
                     margin: 0 auto;
                     padding: 20px;
                     background-color: #ffffff;
                     border-radius: 8px;
                     box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                   }
                   .header {
                     text-align: center;
                     padding: 20px 0;
                     border-bottom: 1px solid #eeeeee;
                   }
                   .logo {
                     max-height: 60px;
                     margin-bottom: 10px;
                   }
                   .content {
                     padding: 30px 20px;
                     text-align: center;
                   }
                   .verification-code {
                     font-size: 32px;
                     font-weight: bold;
                     color: #4a6ee0;
                     padding: 15px 25px;
                     margin: 25px 0;
                     display: inline-block;
                     background-color: #f0f4ff;
                     border-radius: 6px;
                     letter-spacing: 4px;
                     border: 1px dashed #b1c3ff;
                   }
                   .footer {
                     text-align: center;
                     padding-top: 20px;
                     margin-top: 20px;
                     color: #888888;
                     font-size: 0.9em;
                     border-top: 1px solid #eeeeee;
                   }
                   .text-highlight {
                     color: #4a6ee0;
                     font-weight: 600;
                   }
                   .btn {
                     display: inline-block;
                     background-color: #4a6ee0;
                     color: white;
                     text-decoration: none;
                     padding: 12px 25px;
                     border-radius: 5px;
                     font-weight: bold;
                     margin-top: 15px;
                     transition: background-color 0.3s;
                   }
                   .btn:hover {
                     background-color: #3a5dca;
                   }
                   .warning {
                     background-color: #fff8e1;
                     padding: 15px;
                     border-radius: 5px;
                     margin-top: 30px;
                     font-size: 0.9em;
                     color: #856404;
                     border-left: 4px solid #ffd54f;
                   }
                 </style>
               </head>
               <body>
                 <div class="container">
                   <div class="header">
                     <h1>Caballos de Acero</h1>
                   </div>
                   <div class="content">
                     <h2>Verificación de Correo Electrónico</h2>
                     <p>Hola <span class="text-highlight">{{name}}</span>,</p>
                     <p>Gracias por registrarte en <strong>Caballos de Acero</strong>. Para completar tu registro, por favor utiliza el siguiente código de verificación:</p>
                     
                     <div class="verification-code">{{code}}</div>
                     
                     <p>Este código expirará en <strong>5 minutos</strong>.</p>
                     
                     <div class="warning">
                       <p>Si no solicitaste este código, puedes ignorar este correo. Si no reconoces esta actividad, por favor contacta a nuestro equipo de soporte.</p>
                     </div>
                   </div>
                   <div class="footer">
                     <p>&copy; 2025 - Caballos de Acero. Todos los derechos reservados.</p>
                     <p>Este es un correo automático, por favor no respondas a este mensaje.</p>
                   </div>
                 </div>
               </body>
               </html>
             """;
  }

  public static string GetPurchaseNotificationTemplate(User user, MotoInventory moto, Branch branch, Bill bill)
  {
    return $$"""
             <!DOCTYPE html>
             <html>
             <head>
               <meta charset="utf-8"/>
               <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
               <title>Confirmación de Compra</title>
               <style>
                 body { font-family:'Segoe UI',sans-serif; background:#f9f9f9; margin:0; padding:0; }
                 .container { max-width:600px; margin:20px auto; background:#fff; padding:20px; border-radius:6px; box-shadow:0 2px 8px rgba(0,0,0,0.1); }
                 h1 { color:#4a6ee0; }
                 .details { margin:20px 0; }
                 .details p { margin:4px 0; }
                 .btn { display:inline-block; background:#4a6ee0; color:#fff; padding:10px 20px; border-radius:4px; text-decoration:none; }
               </style>
             </head>
             <body>
               <div class="container">
                 <h1>¡Compra Confirmada!</h1>
                 <p>Hola <strong>{{user.name}} {{user.last_name}}</strong>,</p>
                 <p>Gracias por tu compra en <strong>Caballos de Acero</strong>. Aquí los detalles de tu motocicleta:</p>
                 <div class="details">
                   <p><strong>Bill ID:</strong> {{bill.id}}</p>
                   <p><strong>Total factura:</strong> {{bill.amount}}</p>
                   <p><strong>Inventario ID:</strong> {{moto.id}}</p>
                   <p><strong>Estado:</strong> {{moto.status}}</p>
                   <p><strong>Customizaciones:</strong> {{moto.customizations ?? "N/A"}}</p>
                   <p><strong>Kilometraje:</strong> {{moto.km ?? "N/A"}}</p>
                   <p><strong>Sucursal:</strong> {{branch.name}} ({{branch.city}}, {{branch.country}})</p>
                   <p><strong>Dirección:</strong> {{branch.city + ", " + (branch.address ?? "N/D")}}</p>
                   <p><strong>Fecha compra:</strong> {{bill.created_at}}</p>
                 </div>
                 
                 <p style="font-size:.8em;color:#888;">
                   Si no realizaste esta compra, contacta a soporte inmediatamente.
                 </p>
               </div>
             </body>
             </html>
             """;
  }

  public static string GetStatusUpdateTemplate(User user, MotoInventory moto, Branch branch)
  {
    return $$"""
             <!DOCTYPE html>
             <html>
             <head>
               <meta charset="utf-8"/>
               <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
               <title>Actualización de Estado</title>
               <style>
                 body { font-family: 'Segoe UI', sans-serif; background:#f9f9f9; margin:0; padding:0; }
                 .container { max-width:600px; margin:20px auto; background:#fff; padding:20px; border-radius:6px; box-shadow:0 2px 8px rgba(0,0,0,.1); }
                 h1 { color:#4a6ee0; }
                 .details { margin:20px 0; }
                 .details p { margin:4px 0; }
               </style>
             </head>
             <body>
               <div class="container">
                 <h1>Actualización de Estado</h1>
                 <p>Hola <strong>{{user.name}} {{user.last_name}}</strong>,</p>
                 <p>El estado de tu motocicleta ha cambiado. Detalles:</p>
                 <div class="details">
                   <p><strong>ID Inventario:</strong> {{moto.id}}</p>
                   <p><strong>Estado:</strong> {{moto.status}}</p>
                   <p><strong>Placa:</strong> {{moto.license_plate ?? "N/A"}}</p>
                   <p><strong>Kilometraje:</strong> {{moto.km ?? "N/A"}}</p>
                   <p><strong>Sucursal:</strong> {{branch.name}} ({{branch.city}}, {{branch.country}})</p>
                   <p><strong>Dirección:</strong> {{branch.address ?? "N/D"}}</p>
                 </div>
                 <p>Si tienes alguna duda, contacta a nuestro soporte.</p>
                 <hr/>
                 <p style="font-size:.8em; color:#888;">Este es un correo automático, por favor no respondas.</p>
               </div>
             </body>
             </html>
             """;
  }

  public static string GetReadyToPickupTemplate(User user, MotoInventory moto, Branch branch)
  {
    return $$"""
             <!DOCTYPE html>
             <html>
             <head>
               <meta charset="utf-8"/>
               <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
               <title>Tu motocicleta está lista para recoger</title>
               <style>
                 body { font-family:'Segoe UI',sans-serif; background:#f4f4f4; margin:0; padding:0; }
                 .container { max-width:600px; margin:20px auto; background:#fff; padding:20px; border-radius:6px; box-shadow:0 2px 8px rgba(0,0,0,0.1); }
                 h1 { color:#4a6ee0; }
                 .info { margin:20px 0; }
                 .info p { margin:6px 0; }
                 .btn { display:inline-block; background:#4a6ee0; color:#fff; padding:12px 24px; border-radius:4px; text-decoration:none; }
               </style>
             </head>
             <body>
               <div class="container">
                 <h1>¡Tu motocicleta está lista!</h1>
                 <p>Hola <strong>{{user.name}} {{user.last_name}}</strong>,</p>
                 <p>Tu motocicleta ya está disponible para recoger en nuestra sucursal. A continuación los detalles:</p>
                 <div class="info">
                   <p><strong>ID Inventario:</strong> {{moto.id}}</p>
                   <p><strong>Placa:</strong> {{moto.license_plate ?? "N/A"}}</p>
                   <p><strong>Kilometraje:</strong> {{moto.km ?? "N/A"}}</p>
                   <p><strong>Estado:</strong> {{moto.status}}</p>
                   <p><strong>Sucursal:</strong> {{branch.name}}</p>
                   <p><strong>Ubicación:</strong> {{branch.city}}, {{branch.country}}</p>
                   <p><strong>Dirección:</strong> {{branch.address ?? "N/D"}}</p>
                 </div>
                 <p>Por favor, preséntate con tu identificación y este correo en la sucursal para completar la entrega.</p>
                 <p><a href="https://tusitio.com/agendar-recogida/{{moto.id}}" class="btn">Agendar Recogida</a></p>
                 <hr/>
                 <p style="font-size:.8em;color:#777;">Si tienes alguna duda, responde a este correo o contáctanos al +1 234 567 890.</p>
                 <p style="font-size:.8em;color:#aaa;">Este es un correo automático, por favor no respondas directamente.</p>
               </div>
             </body>
             </html>
             """;
  }

  public static string GetCancellationTemplate(User user, MotoInventory moto)
  {
    return $$"""
             <!DOCTYPE html>
             <html>
             <head>
               <meta charset="utf-8"/>
               <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
               <title>Cancelación de Compra</title>
               <style>
                 body { font-family:'Segoe UI',sans-serif; background:#f9f9f9; margin:0; padding:0;}
                 .container { max-width:600px; margin:20px auto; background:#fff; padding:20px; border-radius:6px; box-shadow:0 2px 8px rgba(0,0,0,0.1);}
                 h1 { color:#e04a4a; }
                 .details { margin:20px 0;}
                 .details p { margin:4px 0;}
                 a { color:#4a6ee0; text-decoration:none;}
               </style>
             </head>
             <body>
               <div class="container">
                 <h1>Compra Cancelada</h1>
                 <p>Hola <strong>{{user.name}} {{user.last_name}}</strong>,</p>
                 <p>Lamentamos informarte que tu compra ha sido cancelada. Aquí los detalles:</p>
                 <div class="details">
                   <p><strong>Inventario ID:</strong> {{moto.id}}</p>
                   <p><strong>Placa:</strong> {{moto.license_plate ?? "N/A"}}</p>

                 </div>
                 <p>Si tienes alguna duda o deseas más información, contáctanos a <a href="mailto:soporte@caballosdeacero.com">soporte@caballosdeacero.com</a> o al +1 234 567 890.</p>
                 <hr/>
                 <p style="font-size:.8em;color:#888;">Este es un correo automático, por favor no respondas.</p>
               </div>
             </body>
             </html>
             """;
  }
}