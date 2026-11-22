
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Login" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Iniciar Sesión</title>

    <style>
        * {
            box-sizing: border-box;
        }

        body {
            margin: 0;
            font-family: Arial, sans-serif;
            background: linear-gradient(135deg, #e8eef7, #d5e2f3);
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 20px;
        }

        .login-container {
            background: white;
            width: 100%;
            max-width: 420px;
            flex: 0 0 420px;
            padding: 40px;
            border-radius: 14px;
            box-shadow: 0 8px 30px rgba(0,0,0,0.12);
        }

        .logo {
            text-align: center;
            margin-bottom: 30px;
        }

        .logo h1 {
            color: #2459a6;
            font-size: 27px;
            margin-bottom: 8px;
        }

        .logo p {
            color: #777;
            font-size: 14px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        .form-group label {
            display: block;
            margin-bottom: 8px;
            font-weight: bold;
            color: #333;
        }

        .form-control {
            width: 100%;
            padding: 12px;
            border: 1px solid #ccc;
            border-radius: 7px;
            font-size: 14px;
        }

        .form-control:focus {
            outline: none;
            border-color: #2459a6;
        }

        .btn-login {
            width: 100%;
            padding: 13px;
            background-color: #2459a6;
            color: white;
            border: none;
            border-radius: 7px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
        }

        .btn-login:hover {
            background-color: #194582;
        }

        .register-link {
            text-align: center;
            margin-top: 25px;
            font-size: 14px;
            color: #666;
        }

        .register-link a {
            color: #2459a6;
            text-decoration: none;
            font-weight: bold;
        }

        .register-link a:hover {
            text-decoration: underline;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">

        <div class="login-container">

            <div class="logo">
                <h1>Gestión Comercio</h1>
                <p>Ingresá a tu cuenta para continuar</p>
            </div>

            <div class="form-group">
                <label>Correo Electrónico</label>
                <asp:TextBox ID="txtEmail" runat="server"
                    CssClass="form-control"
                    TextMode="Email"
                    placeholder="Ingrese su correo electrónico">
                </asp:TextBox>
            </div>

            <div class="form-group">
                <label>Contraseña</label>
                <asp:TextBox ID="txtContrasena" runat="server"
                    CssClass="form-control"
                    TextMode="Password"
                    placeholder="Ingrese su contraseña">
                </asp:TextBox>
            </div>

            <asp:Button ID="btnIniciarSesion" runat="server"
                Text="Iniciar Sesión"
                CssClass="btn-login" 
                OnClick="btnIniciarSesion_Click" />

            <div style="margin-top: 12px; min-height: 40px; text-align: center;">
                <asp:Label ID="lblMensaje" runat="server" ForeColor="#c62828" Font-Bold="true"></asp:Label>
            </div>

            <div class="register-link">
                ¿No tenés una cuenta?
                <asp:HyperLink ID="lnkRegistro" runat="server"
                    NavigateUrl="~/Registro.aspx"
                    Text="Crear cuenta">
                </asp:HyperLink>
            </div>

        </div>

    </form>
</body>
</html>
