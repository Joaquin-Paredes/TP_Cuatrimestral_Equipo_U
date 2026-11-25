
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="Gestión_Comercio_TP_Cuatrimestral_Equipo_U.Registro" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Crear Cuenta</title>

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

        .register-container {
            background: white;
            width: 100%;
            max-width: 500px;
            padding: 35px 40px;
            border-radius: 14px;
            box-shadow: 0 8px 30px rgba(0,0,0,0.12);
        }

        .logo {
            text-align: center;
            margin-bottom: 25px;
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
            margin-bottom: 17px;
        }

        .form-group label {
            display: block;
            margin-bottom: 7px;
            font-weight: bold;
            color: #333;
        }

        .form-control {
            width: 100%;
            padding: 11px;
            border: 1px solid #ccc;
            border-radius: 7px;
            font-size: 14px;
        }

        .form-control:focus {
            outline: none;
            border-color: #2459a6;
        }

        .btn-register {
            width: 100%;
            padding: 13px;
            background-color: #2459a6;
            color: white;
            border: none;
            border-radius: 7px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            margin-top: 8px;
        }

        .btn-register:hover {
            background-color: #194582;
        }

        .login-link {
            text-align: center;
            margin-top: 22px;
            font-size: 14px;
            color: #666;
        }

        .login-link a {
            color: #2459a6;
            text-decoration: none;
            font-weight: bold;
        }

        .login-link a:hover {
            text-decoration: underline;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">

        <div class="register-container">

            <div class="logo">
                <h1>Crear Cuenta</h1>
                <p>Completá tus datos para registrarte</p>
            </div>

            <div class="form-group">
                <label>Nombre</label>
                <asp:TextBox ID="txtNombre" runat="server"
                    CssClass="form-control"
                    placeholder="Ingrese su nombre">
                </asp:TextBox>
            </div>

            <div class="form-group">
                <label>Apellido</label>
                <asp:TextBox ID="txtApellido" runat="server"
                    CssClass="form-control"
                    placeholder="Ingrese su apellido">
                </asp:TextBox>
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
                <label>Usuario</label>
                <asp:TextBox ID="txtUsuario" runat="server"
                    CssClass="form-control"
                    placeholder="Ingrese un nombre de usuario">
                </asp:TextBox>
            </div>

            <div class="form-group">
                <label>Contraseña</label>
                <asp:TextBox ID="txtContrasena" runat="server"
                    CssClass="form-control"
                    TextMode="Password"
                    placeholder="Ingrese una contraseña">
                </asp:TextBox>
            </div>

            <div class="form-group">
                <label>Confirmar Contraseña</label>
                <asp:TextBox ID="txtConfirmarContrasena" runat="server"
                    CssClass="form-control"
                    TextMode="Password"
                    placeholder="Repita su contraseña">
                </asp:TextBox>
            </div>

            <asp:Button ID="btnCrearCuenta" runat="server"
                Text="Crear Cuenta"
                CssClass="btn-register"
                OnClick="btnCrearCuenta_Click" />

            <div style="margin-top: 12px; min-height: 20px; text-align: center;">
                <asp:Label ID="lblMensaje" runat="server" Font-Bold="true"></asp:Label>
            </div>

            <div class="login-link">
                ¿Ya tenés una cuenta?
                <asp:HyperLink ID="lnkLogin" runat="server"
                    NavigateUrl="~/Login.aspx"
                    Text="Iniciar sesión">
                </asp:HyperLink>
            </div>

    </form>
</body>
</html>
