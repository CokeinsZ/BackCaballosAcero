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

  public static string GetPurchaseNotificationTemplate(string name, int motoId, string status)
  {
    return $$"""
               <!DOCTYPE html>
               <html>
               <head>
                 <meta charset="utf-8"/>
                 <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
                 <title>Confirmación de Compra</title>
                 <style>
                   body { font-family: 'Segoe UI', sans-serif; background:#f9f9f9; margin:0; padding:0; }
                   .container { max-width:600px; margin:20px auto; background:#fff; padding:20px; border-radius:6px; box-shadow:0 2px 8px rgba(0,0,0,.1); }
                   h1 { color:#4a6ee0; }
                   .details { margin:20px 0; }
                   .btn { display:inline-block; background:#4a6ee0; color:#fff; padding:10px 20px; border-radius:4px; text-decoration:none; }
                 </style>
               </head>
               <body>
                 <div class="container">
                   <h1>¡Compra Confirmada!</h1>
                   <p>Hola <strong>{{name}}</strong>,</p>
                   <p>Gracias por tu compra en <strong>Caballos de Acero</strong>. A continuación los detalles de tu motocicleta:</p>
                   <div class="details">
                     <p><strong>ID Inventario:</strong> {{motoId}}</p>
                     <p><strong>Estado:</strong> {{status}}</p>
                   </div>
                   <p>Puedes revisar tu compra en el siguiente enlace:</p>
                   <p><a href="https://tusitio.com/mis-compras" class="btn">Ver Mi Compra</a></p>
                   <hr/>
                   <p style="font-size:.8em; color:#888;">Si no realizaste esta compra, contacta a soporte inmediatamente.</p>
                 </div>
               </body>
               </html>
             """;
  }
  
  public static string GetStatusUpdateTemplate(string name, int motoId, string status)
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
                 </style>
               </head>
               <body>
                 <div class="container">
                   <h1>Actualización de Estado</h1>
                   <p>Hola <strong>{{name}}</strong>,</p>
                   <p>El estado de tu motocicleta ha cambiado. Aquí los detalles:</p>
                   <div class="details">
                     <p><strong>ID Inventario:</strong> {{motoId}}</p>
                     <p><strong>Nuevo Estado:</strong> {{status}}</p>
                   </div>
                   <p>Si tienes alguna duda, contacta a nuestro soporte.</p>
                   <hr/>
                   <p style="font-size:.8em; color:#888;">Este es un correo automático, por favor no respondas.</p>
                 </div>
               </body>
               </html>
             """;
  }
  
  public static string GetReadyToPickupTemplate(string name, int motoId, string location)
  {
    return $$"""
               <!DOCTYPE html>
               <html>
               <head>
                 <meta charset="utf-8"/>
                 <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
                 <title>Tu motocicleta está lista</title>
                 <style>
                   body { font-family: 'Segoe UI', sans-serif; background:#f9f9f9; margin:0; padding:0; }
                   .container { max-width:600px; margin:20px auto; background:#fff; padding:20px; border-radius:6px; box-shadow:0 2px 8px rgba(0,0,0,.1); }
                   h1 { color:#4a6ee0; }
                   .details { margin:20px 0; }
                   .btn { display:inline-block; background:#4a6ee0; color:#fff; padding:10px 20px; border-radius:4px; text-decoration:none; }
                 </style>
               </head>
               <body>
                 <div class="container">
                   <h1>Listo para Recoger</h1>
                   <p>Hola <strong>{{name}}</strong>,</p>
                   <p>Tu motocicleta (ID {{motoId}}) ya está lista para ser recogida.</p>
                   <div class="details">
                     <p><strong>Ubicación de recogida:</strong> {{location}}</p>
                   </div>
                   <p>Puedes venir cuando quieras en nuestro horario de atención.</p>
                   <p><a href="https://tusitio.com/mis-compras" class="btn">Ver detalles</a></p>
                   <hr/>
                   <p style="font-size:.8em; color:#888;">Este es un correo automático, por favor no respondas.</p>
                 </div>
               </body>
               </html>
             """;
  }
}