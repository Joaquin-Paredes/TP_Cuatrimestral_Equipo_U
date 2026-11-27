CREATE DATABASE GestionComercioDB;
GO
USE GestionComercioDB;
GO

CREATE TABLE Categorias (
    Codigo VARCHAR(20) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(250) NULL,
	Estado BIT DEFAULT 1
);
GO

CREATE TABLE Marcas (
    Codigo VARCHAR(20) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
	Estado BIT DEFAULT 1
);
GO

CREATE TABLE Proveedores (
    CUIT VARCHAR(20) NOT NULL PRIMARY KEY,
    RazonSocial VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NULL,
    Telefono VARCHAR(20) NULL,
    Direccion VARCHAR(150) NULL,
	Estado BIT DEFAULT 1
);
GO

CREATE TABLE Clientes (
    CUIT VARCHAR(20) NOT NULL PRIMARY KEY,
    NombreCompleto VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NULL,
    Telefono VARCHAR(20) NULL,
    Direccion VARCHAR(150) NULL,
	Estado BIT DEFAULT 1
);
GO

CREATE TABLE Productos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Codigo VARCHAR(50) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(250) NULL,
    MarcaCodigo VARCHAR(20) NOT NULL,
    CategoriaCodigo VARCHAR(20) NOT NULL,
    Precio DECIMAL(18,2) NOT NULL,
    Stock INT NOT NULL,
    StockMinimo INT NULL,
	Estado BIT DEFAULT 1,
    FOREIGN KEY (MarcaCodigo) REFERENCES Marcas(Codigo),
    FOREIGN KEY (CategoriaCodigo) REFERENCES Categorias(Codigo)
);
GO

CREATE TABLE Ventas (
    NumeroVenta VARCHAR(20) NOT NULL PRIMARY KEY,
    Fecha DATE NOT NULL,
    Cliente VARCHAR(100) NOT NULL,
    Vendedor VARCHAR(100) NULL,
    Total DECIMAL(18,2) NOT NULL,
    Estado BIT DEFAULT 1
);
GO

CREATE TABLE DetalleVenta (
    Id INT PRIMARY KEY IDENTITY(1,1),
    NumeroVenta VARCHAR(20) NOT NULL,
    Producto VARCHAR(100) NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(18,2) NOT NULL,
    Subtotal DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (NumeroVenta) REFERENCES Ventas(NumeroVenta)
);
GO

CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    NombreUsuario VARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL UNIQUE,
    Contrasenia VARCHAR(255) NOT NULL,
    CodigoRecuperacion VARCHAR(100) NULL,
    EsAdmin BIT NOT NULL DEFAULT 0,
    Estado BIT DEFAULT 1
);
GO

INSERT INTO Usuarios (NombreUsuario, Email, Contrasenia, EsAdmin)
VALUES ('admin', 'mikolajewski.04@gmail.com', '1234', 1);
GO