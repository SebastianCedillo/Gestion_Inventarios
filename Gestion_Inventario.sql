CREATE DATABASE GestionInventarios;
GO


USE GestionInventarios;
GO


CREATE TABLE Proveedores (
    proveedor_id INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(50) NOT NULL,
    direccion NVARCHAR(100),
    telefono NVARCHAR(15),
    email NVARCHAR(50)
);
GO

CREATE TABLE Productos (
    producto_id INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(50) NOT NULL,
    descripcion NVARCHAR(255),
    precio DECIMAL(10, 2) NOT NULL,
    stock INT NOT NULL,
    proveedor_id INT,
    FOREIGN KEY (proveedor_id) REFERENCES Proveedores(proveedor_id)
);
GO


CREATE TABLE OrdenesCompra (
    orden_id INT PRIMARY KEY ,
    fecha_orden DATE NOT NULL,
    proveedor_id INT,
    FOREIGN KEY (proveedor_id) REFERENCES Proveedores(proveedor_id)
);
GO


CREATE TABLE DetalleOrden (
  
    orden_id INT,
    producto_id INT,
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (orden_id) REFERENCES OrdenesCompra(orden_id),
    FOREIGN KEY (producto_id) REFERENCES Productos(producto_id)
);
GO

