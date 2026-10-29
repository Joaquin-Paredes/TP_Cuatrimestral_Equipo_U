CREATE DATABASE GestionComercioDB;
GO

USE GestionComercioDB;
GO

CREATE TABLE Marcas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL
);
GO

INSERT INTO Marcas (Nombre) VALUES ('Coca Cola');
INSERT INTO Marcas (Nombre) VALUES ('Arcor');
INSERT INTO Marcas (Nombre) VALUES ('Pepsi');
INSERT INTO Marcas (Nombre) VALUES ('Seven Up');
INSERT INTO Marcas (Nombre) VALUES ('Terrabusi');
GO