Use CentroIntegralSD; 

--Insert de Estado
INSERT INTO [CentroIntegralSD].[dbo].[Estado] ([Descripcion]) VALUES ('Activo');
INSERT INTO [CentroIntegralSD].[dbo].[Estado] ([Descripcion]) VALUES ('Inactivo');

--Insert de Estado_Asistencia
INSERT INTO [CentroIntegralSD].[dbo].[Estado_Asistencia] ([Descripcion]) VALUES ('Asistida');
INSERT INTO [CentroIntegralSD].[dbo].[Estado_Asistencia] ([Descripcion]) VALUES ('No Asistida');

--Insert de Estado_Contabilidad
INSERT INTO [CentroIntegralSD].[dbo].[Estado_Contabilidad] ([Nombre], [Descripcion]) 
VALUES ('Pagado', 'El pago ha sido realizado correctamente.');

INSERT INTO [CentroIntegralSD].[dbo].[Estado_Contabilidad] ([Nombre], [Descripcion]) 
VALUES ('Pendiente', 'El pago aún no ha sido realizado.');

INSERT INTO [CentroIntegralSD].[dbo].[Estado_Contabilidad] ([Nombre], [Descripcion]) 
VALUES ('Rechazado', 'Registro que ha sido denegado por inconsistencias o errores.');

INSERT INTO [CentroIntegralSD].[dbo].[Estado_Contabilidad] ([Nombre], [Descripcion]) 
VALUES ('Vencido', 'Registro con fecha de vencimiento expirada sin haber sido procesado.');

INSERT INTO [CentroIntegralSD].[dbo].[Estado_Contabilidad] ([Nombre], [Descripcion]) 
VALUES ('En Revisión', 'Registro en proceso de análisis antes de su aprobación.');

--Insert de Metodo_Pago
INSERT INTO [dbo].[Metodo_Pago] (Nombre)  VALUES  ('Efectivo');

INSERT INTO [dbo].[Metodo_Pago] (Nombre)  VALUES  ('Tarjeta');

INSERT INTO [dbo].[Metodo_Pago] (Nombre)  VALUES  ('Transferencia');

INSERT INTO [dbo].[Metodo_Pago] (Nombre)  VALUES  ('Crédito');

--Insert de Receta
INSERT INTO [CentroIntegralSD].[dbo].[Receta] ([Fecha_Creacion], [Nombre_Receta], [Observaciones_Pacientes], [Duracion_Tratamiento]) 
VALUES ('07-02-2025', 'Receta para hipertensión', 'Tomar con abundante agua', '30 días');

INSERT INTO [CentroIntegralSD].[dbo].[Receta] ([Fecha_Creacion], [Nombre_Receta], [Observaciones_Pacientes], [Duracion_Tratamiento]) 
VALUES ('07-02-2025', 'Receta para alergia', 'Evitar alimentos que desencadenen reacciones', '15 días');

--Insert de Modificacion_Receta
INSERT INTO [CentroIntegralSD].[dbo].[Modificacion_Receta] ( [Fecha_Modificacion], [motivo_modificacion]) 
VALUES ('07-02-2025', 'Ajuste de dosis');

INSERT INTO [CentroIntegralSD].[dbo].[Modificacion_Receta] ([Fecha_Modificacion], [motivo_modificacion]) 
VALUES ('07-02-2025', 'Cambio de medicamento');

--Insert de Servicio
INSERT INTO [CentroIntegralSD].[dbo].[Servicio] ([Nombre_Servicio], [Precio_Servicio], [Especialidad]) 
VALUES ('Consulta General', 10000, 'Medicina General');

INSERT INTO [CentroIntegralSD].[dbo].[Servicio] ([Nombre_Servicio], [Precio_Servicio], [Especialidad]) 
VALUES ('Terapia Física', 75000, 'Fisioterapia');

INSERT INTO [dbo].[Servicio] ([Nombre_Servicio], [Precio_Servicio], [Especialidad])
VALUES ('REMBOLSO', 0.00, 'Finanzas');

--Insert de Solicitud_Receta
INSERT INTO [CentroIntegralSD].[dbo].[Solicitud_Receta] ([receta_solicitada]) 
VALUES ('Receta para antibióticos');

INSERT INTO [CentroIntegralSD].[dbo].[Solicitud_Receta] ([receta_solicitada]) 
VALUES ('Receta para analgésicos');

--Insert de Tipo_Registro
INSERT INTO [dbo].[Tipo_Registro] (Nombre, Descripcion)  
VALUES  
('Ingreso', 'Registro de ingresos provenientes de ventas o servicios.'),  
('Egreso', 'Registro de gastos operativos o inversiones de la empresa.'),  
('Pago de Nómina', 'Registro de pagos realizados a empleados.'),  
('Impuesto', 'Registro de pagos relacionados con impuestos y tributos.'),  
('Anticipo', 'Registro de anticipos realizados a proveedores o empleados.'); 

--Insert de Tipo_Transaccion
INSERT INTO [dbo].[Tipo_Transaccion] (Nombre, Descripcion)  
VALUES  
('Venta', 'Transacción de venta de bienes o servicios a clientes.'),  
('Compra', 'Transacción de compra de productos o servicios a proveedores.'),  
('Pago de Nómina', 'Transacción de pago de salarios a empleados.'),  
('Pago de Impuestos', 'Transacción de pago de impuestos y tributos fiscales.'),  
('Anticipo a Proveedor', 'Pago anticipado a un proveedor antes de la entrega del producto o servicio.'),  
('Reembolso', 'Devolución de dinero por compras o gastos previamente realizados.'),  
('Depósito Bancario', 'Ingreso de dinero en una cuenta bancaria de la empresa.'),  
('Retiro de Efectivo', 'Extracción de dinero en efectivo para caja chica o pagos en efectivo.');  

--Insert de Descuento
INSERT INTO [dbo].[Descuento] ([Nombre_Descuento],[Codigo_Descuento],[Porcentaje_Descuento],[Limite_Usos],[Compania_Afiliada],[Activo],[Fecha_Creacion])
VALUES ('Descuento de Verano','SUMMER2025',15.00, 100, 'Compañía XYZ', 1, GETDATE() );

INSERT INTO [dbo].[Descuento] ([Nombre_Descuento],[Codigo_Descuento],[Porcentaje_Descuento],[Limite_Usos],[Compania_Afiliada],[Activo],[Fecha_Creacion])
VALUES ('Descuento Navidad','XMAS2025',20000, 50, 'Compañía ABC', 0, GETDATE() );

--Factura
INSERT INTO [dbo].[Factura] ( [Id_Descuento], [NumeroRecibo], [FechaHora], [MetodoPago], [CedulaCliente], [NombreCliente], [Subtotal], [Descuento], [Impuesto], [TotalPagado], [Descuento_Id_Descuento])
VALUES (1, 'RC12346', GETDATE(), 'Efectivo','987654321','María López',150.00, 0.00, 22.50, 172.50, NULL );

--Insert de Servicios_Brindados
INSERT INTO [dbo].[Servicios_Brindados] ([Id_Factura],[Id_Servicio])
VALUES (1,1);

--Insert de Metodo_Pago_Utilizado
INSERT INTO [CentroIntegralSD].[dbo].[Metodo_Pago_Utilizado] ([Id_Factura], [Monto], [Id_MetodoPago]) 
VALUES (1, 150.00, 1);

-- Insert de AspNetUsers (Esto es porque medico necesita un id)
INSERT INTO [CentroIntegralSD].[dbo].[AspNetUsers] ([Id], [Nombre], [Apellido], [Edad_Paciente], [Direccion], [Cedula], [Imagen], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) 
VALUES (1, 'Carlos', 'Martínez', 35, 'Av. Principal 123', '123456789', 'data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAkGBxIQEhUREhIVFhUXGBUVGBcYFhUVGBcWFRUWFhYVFxUYHSggGB0lGxUVITEhJSkrLi4uFx8zODMtNygtLisBCgoKDg0OGhAQGy0lICUvLS0tLS0tLS8tLS0tLS0tLS0tLS0tLS0tLS0rLS0rLS0tLS0tLS0tLS0tLS0tLS0tLf/AABEIAKgBLAMBIgACEQEDEQH/xAAcAAABBAMBAAAAAAAAAAAAAAAAAwQFBgECBwj/xAA/EAABAwIEAggEBAUDAwUAAAABAAIRAyEEBRIxQVEGEyJhcYGRoTKxwfAHFFLRQmJykuEjgvEWotIVJDNTsv/EABkBAAMBAQEAAAAAAAAAAAAAAAABAgMEBf/EACQRAAICAgIBBAMBAAAAAAAAAAABAhEDIRIxQQQiUWETFHEy/9oADAMBAAIRAxEAPwDtiEIQAIQhAAhCEACEIQAIQhAAhZQgCgfi3ijTwukGNUME8XPMf/gP/uXO8uxVY4duHoBraFJ5L3S1vWVCZ1OkyTpIho2AEq8fjRTmnRN7Fx7ie/0jzVRzbBvw+AaGnQS9zjIbpcSXDkSSIssJd0ax6srGeZySS0G3Cb+8X9Aq2+qSU5rSSZI8gB7BBwJie6VpFKJnLlIbip9/NAfHy+i1NMhalWZg3dZL7R4e0/ugNRpTA2a72WgN1sGJUUDv4FKwoUo1iLDhfxPefIeid4OuBUa4mWsvy1EmSY89kwLSAssdpUtWV0TOa5jLjeS6HOIO5gwNthb1U/8Ah501OCrHXem6QRyv6SFRq9a4P3Mk/VZw74I7/WJH7I46Dls9UdGsb11LVM7T/VHaA58D/uUuub/h/mzaTW03PDRUHZmQ0vAFhy5DwC6LSqBwBHFKErRUlTNkIQrECEIQAIQhAAhCEACEIQAIQhAAhCEgBCEJgYe8NEkgDvsm7swpD+MeUlU/pFmLnYp9KeyzSAPFocT43Qym4jsx5mFhLNTpG8cOrZbDmtLmfRanOKXf7fuqPiqrm2MtePhPA9xI3HyTTD5swtLiYidQPAjcLL9hmv62rOijNafM+37p5TqBwkEHwK5pluKNRgffU/tRyB+EekeafZbmNSjDpkNcQ7vYTMe/srjnd7IlgromenWUjE06TTt1rGu/pedJPuFzD8Z6VRj6LL6CzVp2AeSQ7xMgrt72tqs5ggEH3BXGvxpe6rjaVAAwGN25uJWrVOzDbVFF6L5Mazy93wMuTw1HYfVWo5c0iAFJ0Mu/L0WUmCYEuMXLjv8AskqFWLEEeRC5Ms22d2HHxiVLMshIkjxUDUy54O0rrbaVNwuQlMLlrJkAH0VQzSQsmGEjnGUdFa9aDoIbzMCPqpb/AKFeLE3mxHLmf2XSWDSIAC3aJ3VPJJkLHFeDnWG6EEXdfhH1Sz+ibRAuuguAhR+JYolKXyXFR+Dn2bdGHBo0AH73KrGNy91I9rwXYS0EKvdKsC3qiQO8qoZWhTxRkjljylsKBILtgsYinBI5JNo5rs7R53TLVSzgvDWgTDmjTw07x/cG3Xeug9WcJTBeXkCCSZMybeA2k7wvM+Eq6HAi3n6+C7B+E+bCtUqsaIDWtcbul0HSTJcZgcNlnXF6NLvs6yhZWFoIEIQgAQhCABCEIAEIQgAQhCQAhCEACFlCYHOc+pzmNXwpk+bGp0aZcIJtySGcPnHVz3sb/bTb/lOaUkbE+S4Zf6Z2x6RXM4pPoNL2kvp7lp7Rb3tO5HduuddJszcCalM9moIePr6W8l2PFM1AhcVznKD+fOH2YSHgcNBuQPMEJY4pS2aynJxpdnQuheY1K1MPNPS2LEm5HMN5KwYJ4e6o0/pB+aiKFZtChJsAAkuj+MLtdV1tXwz+kbHzlQmXKF2X/ozmXw0nGzp0/wBQ3HnBVRz/AAvWZniKrhIpimxs7ToafafdNKeedTTNSfgeXN8WukXWvTbpBSo1qj2idRDo5uLB8XkAPJbKfKNHO8XGdjj8m9/a6x5/pDQPKbnxQyjBguM8nCCqnk3TAPfpeYnvi/JXXCYttRtyHeMFZuNdmila0bPp9m4BSlGi3hbwJCWLRCGUuIKKFYPpX3PstocOM+S3aD3eiy/V+kev+FVE2JOeTwSD2SnIM7ghYe2AgaZH1BFlD9JmTR8FPPp6iozPqWqmW91lKRV2cgx9MhxlNQ6FK5nSLXEbhRbmLvg7R52RUxZjdX+F0P8AB4n88xrZMsqB1gIBHv8ACud0DwH2V2L8DsnLX1sQ4bAUx4mCY52+aJCideWFlYTGCEIQAIQhAAhCEACEIQAIQhIAQhZQAKs5h0qaHFlKDBI1byRvCW6dZx+UwdR4MPd/ps/qdufISfRcYpZyKQl0rHNNrUTqwYVJOTOh1s3aXk6W6nGXOgAk7STx2CWrZm6BBjwXLMT0pYTqa6/gVc8qzIVAL2IC5XyXZ0KMfBN065dciCqV07ohujFAXpmHf0OsfQwfVXRpHBROfYMVKbwRIcCD5iJQ9bHHspdXN212tkksBHZ/U7gO9TGZYGs6gHteG7dgDhtvxKqOSMPXNpuAaWOiDql3CRaItO89ob8OmZnQf+VJpjtAS3xAVSjTpGincbKDnOLdTLKBGxBIP8t7+iaYusaz3OebG57/AD7lGZjiX1qoc4XJA3B8dk4xdFzmgDzPAd5WqilRg5uTf0IV8vG9J0/ymPZTfRPOX0nhlQuieP8AlJdFeipxNbq3FzRGouYfhvvezh4FLY/AVMHUNGtBI2eBZw4Hu+ivItV2Y4ZRc9aOkMxIIF1rVzNtMSTYCT3KH6N1HVGA7j9ktnWB1NIIjYrks63FXQhX6eYdroJd4AfVP8P0sovYHBxAPMEbcFScdkbbuLwJ5xbwUfhsm1ns1gY2utVxa7MXGSfR1TDZ9TqRpIPzHqnb3g2XLfyNVo3MC9pny5KbyvOajHCnUIg2Ena1u/x8Uh8S7aYEqJzUEyRuNvPgnjMTqbMphiDNjspbHFHOc6pS4nlf79FB1GKczR0Pqgcyoh7L27l2Y+jizbkK5bQntbmCR3aeJXo/8O8AKOAo83jrHHmXAAH+0NXCuh2SuxNdlJu7+yTfsgw4n0BK9LUKIY1rGiGtAaByAEAKu2Zro3QhCoAQhCABCEIAEIQgAQhCABCEJACysKB6c5gcPhHOaYLnMpyOGo39gR5pN0rHFW0iD/ETDNx1NlOlVAcxxNxLXSI3F7EC8c1T8i/D5jndZjsSzQyXGizV2w28Oe4Ds2uGgzzClcFiAQLp6a4Ighcn5d2zu4NR4pnPaGVmpUqVnNH+o9z4j4Q4yGgcgIHklaFQ4V+l1mm7TwB5K7OwDXXbY8uBUVnWUmowt6uT4j5lZttvZtFpaRL5fiBUYHBOmNDrEKD6N4WtRphjmm25JHpup1oIQiH2UXpHlH5fEUqkHQ4uaDyPxBpPqrtk7tVMDuSuKpsqsLHjU1wuD92KYZPhxQJpBxIGxJl0HaUN00XfKDTKH0g6Nflsc17b0qhc4H9Lryz1Mj/Cn8JlTHMLU96dGKTHcqjIPiY+qxl9WQE5ybonHGk6GGHyJlN2ttje4JafZJ5tgw+HOLnOFhJJkG0QVOvjfgk8twprVNZHZabJJsql2x30dy78rRaDvF1tij1xICdZg4QACm+DbBTfdGa6s530sy6o14YSSCbkWbB2vx24KNyTo86tVaxodqOmC1waW/rLgRsPoV0fN8vLndrZJ4HIGDcHfVYmCe9a48nHVGWXFzqVkFiS/B1Rh8Xpe0xorN9mubz+7pxicvaWgsa0zcGJBHkbKw4jo5QqEFw28/WfFI0st0HSNvBZz+jXG9U3YywDnNEGT3/QynD3SLperS07FMMe/S0nuKgs55jTNWoebj8z+xSTaUyfu/2Vl1MuraRdxI9Tv81d8s6Mt6siqbuABjcHiQV1uajRxRg5tk/+CWUx1uIdwhjZ4TJPtHqusLk2S53+QeyhSrN0agTTOkl3A8JmAusAzdXjmpEZMbh2CEIWhmCEIQAIQhAAhCEACEIQAIQhIDKpX4gPZiqYwzahaWvDi4AES0OGn/unyV1C45QxZk6nGdTgb8Zusc8mlXyb4IW7+CL/APSsZQuwiqP7HR4Gx9QlcN0nYHClXaaT+TxpnwndTmHxxm5T2qyliG6atNj28nNDh7rlVHW2xphsW12xkJ6Kkb3TOj0cwwcDTDmx/CHu0/2k2Ug7A2RQWjLNJ4IkJpdlvdaA8UrKcSQDFF69NbUTaAD6p/RrcFH49w1eKUgg/Aj06o6sK5w4FrvRwJ9kxy8jSIUo4jEUHUX7lpaPT5qsZJiHaIO7ZafFpgpydovGqtE9ZxDSY2H/AArDhmNazSwWXOs2xjqb2vBsCPmrDhukVPQbxaTfuunDWycsW+h/VqEvhO8J2iIVTw3THDOq9SS4OcYBLSATw3urHldb/VIHKU6aeyXTTrwS9enqSNOhGyc03SsPK0oxE3MTaqA3Zb1KpF0wxNbvUtlqIhiqnJQec1oYR3T5fZT+s4quZliNTiL6YMn1MT98VC2y3pGvRjDMdUdWNwIaJHFWl+KY93VA3ABPnwlR2RZbpw/CSSQdtzMp5i8KKdRoYJc5rZHIpzdsMUVQhh+j5xeOY1tg0NJMbCTf0+i7W1sCBwsqr0FwcCrXIu4hgPczePP5K1rqwxqNnJ6idyr4MIWVhbHOCEISAEIQgAQhCYAhCwkBlCEIA1rVmsaXuIDRck8FwfP8oxbqtV9FzC1z3uDZIOlziQNo2Oy6T+JOZdXTpUQb1HFxH8rB/wCRHoqRQxpHGVy55+6js9Pj9vIqlLOMRhDpxVF7W/qiQO/U2Qp7JOkVOq4aHTKn6OMDhcSlstZR1HTTa0neGgT6LFtM3Sku9m4rndO24mwKVxFFg2smLmxYJbQaZtinpGncJV8EQkW2SYLoxUcQ6UjmLpaDxsnNVkiR9lN8U0mnMeiTKQzbW03HC/8AhV2himsqV7gdtzhw3Adx8VKvqwY5yqJi3ziat4uL/wC1qrHHlaDJLhTHmZZg+qS0HszwG55TdL5XXZTb2g5zzeJmAOfI9yzgmUYLQ8gnjw8L+V09wmRPuQ5ptZsjfgPv9lq2qoip3Y8wVWhiRDmsa8AEOLQYPIO3t4q4dH8E1jS/VqJ4/QLnDsA6i8hwvvAm22o7Xi1u8Ky9H89NNsPMgNH7fuUqV2NyfGi5tkWWlapZJ4THMrND2ncd33vZa13JMhDWtUTDE4oCb3SleuNJggm/sYVYzKuS7QDaCHGdhAn2gKezQVx2ZQXCZjgO/h5qMqCdFNx+I37gILo8rJPrhOrexie5OMjrB9cl5loGkHhJiYHkrSpWQ9ui0URVIGhgLIsZsBHKUpg6b3PAbqdVdYSIaJ4zvsnmX4M1T/o0yfD4R4nYK1ZTlraDpJBqHc8AOQ/dKGNyHkyqComMra2hSZSAs0RPM7k+Zkp4MQE1aIW5hdy0ec3exz1q2Dwm2oAbrBqgd55IEPFhMT1h2hvzWWdYLzKAHqFpSqagt0ACEIQALCEIAysrCEAc2/FvBPa6jjBOloNN0fwmS5p85cPIc1yr/qkuqQGEj9Q39OK9H571RoVG1g009BLw7bSLmV51p4JrHVX4ek5wlxY113BpNp5mLxvaFz5Ixu2dWKcmqXgs+S49tRpgj6+Y4KWwb9LrqkYWg6k01mv1h0aoEARNg3cEXEG6uGVPFemHXB9FzSjTOuMtWTgcSN7LDGTxSFJpjeyXm0IRLNXCPotHN/dZdcW+/wB1nwKTBaMUSdtkhp+Jt0tp5eyK7Sdhcc1LGVrHN0nwKoeJw4fiagMjtDu4BdJx2H1XjbePqqTnmXObW1D+IRP8w4LTDKmx5o8kv6O8J0epOAio5p8Z9intHo5WF2VWnxb9QVVKuNrU3do7fS30Upk/SR7XhrgbkbEjfuWzjLvshZYXVUWA0sVTaQ+m2q0iJaRrHcA6OWyh61ZrHmNTSYJa4FptwAMTsrXhMa+obF0cf+VIVsOyoIeA7xusrRckV3J6xpPEuhplwJ/mvpPLgVK5pmDg/TJAcGuG15cJjlx9lEZ4WsY0H9XmdI5+Epli8WXQ0OALR6mW787avNHZA5xeOkloMcZ5mdvvuUMahDnB0m4+n1hO+rAJfM7WnaXSRH3sk8c4jVI759d/L6JoYlTYNQJA3k8o3Py9k7y4gwW8Z+dyo/rA5ultzYTzLt/afVPg3SKdNggvc1g42Jv7BXWqM09tnWMlPUYVpG5E+Jdt9FMYGkQJd8Ruf2UP1RLqdIbMAefk0fM+Smqbua6Ecct7HjClLbpCmOJ2Sb6uq3D5qiBRzte23NOaTA0WCbsKUFTkmAuhJhy2DkWBsLXS0pCVsxyAFEIQgAWELCAN0EoUR0kzhuFpOebuiGjm6DA+p7gk3SsaVukVD8SM7Lv/AGlLiQ6qR+mQWs87H05qmUMQ6i+G03Ppthrw1up/WOGoRyAED/eFIYem6o91R/ac8lxPjxTvBtbSqGiNTnODqznHgS6AD5SB3MXDKfJ2dyjxioojMto9c6uTS6vthhGqZIaCSYtqvBjklMqJo1OqNuI8O5TuV5d1LA0nUS57ydpL3Fx+cJHpHl5LRVYO0y/iOLUmrKg6VMfDgtnEATxUbl+MFVoM7p23dIKNXTudvv3Wr9X+EtUbsJ7/AB2P7o6njP8AgdyTHZqxpndbmnP/ACjSRw3+aULY/ZILGtajaR98UxxuWNqtM7cDsQQpkEbFZqsA7kUNSOZZlhjScW1mWuWu4Ota/DvSmUUWPN2yZ5bTb2V8x+EpVW6XtDmxdUrGZZVwrnVKI1N/SbuEGRfitFJPQ/suGEFBnEXsfKffdRmY49ga7TImdJg39/G3JVen0lIuIk2cI+ICP8+aY4nNXOabntXaSbggiBHgPuVf4zJz+x9icwNXU5xteBYx8V471EPxDmlpa7f2kST47+6jziofqnyjjy9kliMSCTHOVqsdGMspMVsaCdckkcO8Rc90+yTq5idN7uM78BEAAKLpVHOgNBKl8BgDIdUuflx802lHsIuWR6HmVYeBLmxO3d929FYujOD6/MKTf4aTTUPibD5KPoNVu/DTB2xWJ5u6tp7mC/8A3F3os4PlOzfKlDHSLfg3Xc/i4+wsPkpCgJPzUXROn0T8PhobzGp3hwC3RxMUxFedtuHf3rVjk3HacB5nwTvDxJedhYeKYhfa3FbtKQDuKVYJTEKhy3bKTb/KJ7+HlzW/V8XFAG3mECUman6QkXlx4oESQKExwEgmTunqYAsFZK1QBmtVDQSTAH3K5F0izV2MxBLRNNphg2EcXnvPyhWbp/nkRhWTLrvI4NmzfNU/CtgwB9/YXJnyX7UdeDHXuZIZfQgST9AEpgcMRUq1XEEvLQ0i/wDptENE+JcfNQvSOg91MNDy0P00gxti57zEuduWgSYEfDdTOQ5UcK11MP1U5ljTcsEXbPET6SsktFuTcqJqmEo4SFrTFlvCoCnZhRODql4/+Fxk/wAjjufAqVw+MDxY7qQxlFrwWuEg2hUDMW1cvfqbLqJO25b4fspr4NE7Wy9UmDh6pyGqu5TnQqMa+4a6dJIIB0kggE8iCFNU8W1w3UiaHgAK1eY+7DyTdtSClmQ7cpiozpB5rSAt3vjZaOvy7kAIPANtk0q0ZFxMffFOatLvukHB3eVDNIlQz7o6yrL2nS4cefiqPj8DVpu0keHIjuXXq+F1GRbhHdx8VGZllbXiCyT7eMLTHmcdMjJhjP8ApyxuFcU7w+X81P4zJHUzLR2eR39UlSaNl0fltaMV6dJ7EsNhI2UpQodyRo4bkVIUpWUpHXCKRriX9Wxz+Qt4rpvRXL/y+Ap0z8RGp3e4u1O91zag04jFYfDD+J4c7+in2j6xHmut4mm7rGtHwCk6R/M5wDT6B3qtcK1Zy+pnbSEQ2XNHMp9ixE98egATXBiXtW+Zvkho4mPUwtTmZjDGGl3FxgeCdAgADgPc8SkaLQTHBg9/v5ramdbu4JiHFBmq/D72TwU/Tl97rRpACTfVJ7gmIcufCRLwdykmwtwAgDbWPsLGsc/ZZjuWjmIEbU3tDh2vonyiXU+5SoQBlarK1QBxWpiXYmpUquPac6ZvadgO4WHkpHA0NFyR4+F0IXnPs9N6VCDscMRUaMOwPNMkiq6eraYLSW//AGGCRa291YmCw5oQtGqdGMHasdsFls4IQmA2qqBz2gHsIIQhQy49jjofRYMA2g9oc3VVse+q8yOR70nissdSOqiS5nFhPaHgeI9/FCFu4qS2Y83GToRONm2xG4MiPFOsLiyCsIXK9M6/A5r43SNX36LWnjQbTusoSsOKoUF/uN1h4MWgAeaEJkiLmahdJhmm254yhCkpCNSjrmd/KIUNmWSB9x8Xdv8A5WEITroZChr6boqAiNjzTh2LDWkyhC2XuC6JP8JaBxGMrYk7Mb1bfFxBd7ALreIb2nHuA9JP1QhdiR5knbsa4BvaSONdFQHlJ9JQhHgPJrRqHQAN3b+Cf0YaIQhCExZpJSzKI43QhUIXawclvCEJiCFo5pWUIAa1Wu4HyKfhYQkALCwhAH//2Q==', 'carlos.martinez@example.com', 1, 'hashedpassword123', 'stamp123', '555-1234', 1, 0, NULL, 0, 0, 'carlos.martinez');

INSERT INTO [CentroIntegralSD].[dbo].[AspNetUsers] ([Id], [Nombre], [Apellido], [Edad_Paciente], [Direccion], [Cedula], [Imagen], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) 
VALUES (2, 'Ana', 'González', 28, 'Calle Secundaria 456', '987654321', 'data:image/jpeg;base64,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', 'ana.gonzalez@example.com', 1, 'hashedpassword456', 'stamp456', '555-5678', 1, 0, NULL, 0, 0, 'ana.gonzalez');


--Insert de cita
INSERT INTO [CentroIntegralSD].[dbo].[Cita] ([Id_Medico],[Nombre_Paciente], [Estado_Asistencia], [Hora_cita], [Descripcion_Complicaciones], [Sintomas], [Fecha_Cita], [Modalidad]) 
VALUES (4,'Nicole Hidalgo', 'Presente', '10:00', 'Sin complicaciones', 'Dolor de cabeza y fiebre', '2025-02-07', 'Presencial');

--Insert de Medico
INSERT INTO [CentroIntegralSD].[dbo].[Medico] 
([Id_Cita],[Id_receta], [Especialidad], [Horario_fin], [Nombre], [Horario_inicio], [Fecha_creacion], [Id], [Estado_Asistencia_Id_Estado_Asistencia])
VALUES 
(1, 1, 'Cardiología', '18:00:00', 'Dr. Carlos Martínez', '08:00:00', '2025-02-03', 1, 1);

INSERT INTO [CentroIntegralSD].[dbo].[Medico] 
([Id_Cita],[Id_receta], [Especialidad], [Horario_fin], [Nombre], [Horario_inicio], [Fecha_creacion], [Id], [Estado_Asistencia_Id_Estado_Asistencia])
VALUES 
(1, 1, 'Psicología', '18:00:00', 'Dra. Ana González', '08:00:00', '2025-02-03', 2, 1);

--Es un cambio ya que agregue una atrubuto a la tabla Medico *El 2 es hombre y el 3 es mujer*
UPDATE [CentroIntegralSD].[dbo].[Medico]
SET Imagen = 'https://static.vecteezy.com/system/resources/thumbnails/026/407/945/small/guy-man-grey-african-joy-blue-black-portrait-fashion-red-background-american-american-photo.jpg'
WHERE Id_Medico = 2;

UPDATE [CentroIntegralSD].[dbo].[Medico]
SET Imagen = 'https://images.pexels.com/photos/733872/pexels-photo-733872.jpeg?cs=srgb&dl=pexels-olly-733872.jpg&fm=jpg'
WHERE Id_Medico = 3;

--Insert de Roles 

--Rol secretaria
INSERT INTO [CentroIntegralSD].[dbo].[AspNetUsers] (
    [Id],
    [Nombre],
    [Apellido],
    [Edad_Paciente],
    [Direccion],
    [Cedula],
    [Imagen],
    [Email],
    [EmailConfirmed],
    [PasswordHash],
    [SecurityStamp],
    [PhoneNumber],
    [PhoneNumberConfirmed],
    [TwoFactorEnabled],
    [LockoutEndDateUtc],
    [LockoutEnabled],
    [AccessFailedCount],
    [UserName]
)
VALUES (
    NEWID(), 
    'María Carmen',
    'Maroto Hidalgo',
    22,
    'Guanacaste, cañas',
    '11-53456789',
    'https://blog.oxfamintermon.org/wp-content/uploads/2018/05/derechos-de-las-mujeres.jpg', 
    'MMaroto01CentrointegralSD@gmail.com',
    1, 
    '123456JMaro+', 
    NEWID(), 
    '1234567890',
    1, 
    0, 
    NULL, 
    0, 
    0, 
    'María.Carmen' 
);

INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT u.Id, r.Id
FROM AspNetUsers u, AspNetRoles r
WHERE u.Email = 'MMaroto01CentrointegralSD@gmail.com' 
AND r.Name = 'Secretaria'


--Rol medico
INSERT INTO [CentroIntegralSD].[dbo].[AspNetUsers] (
    [Id],
    [Nombre],
    [Apellido],
    [Edad_Paciente],
    [Direccion],
    [Cedula],
    [Imagen],
    [Email],
    [EmailConfirmed],
    [PasswordHash],
    [SecurityStamp],
    [PhoneNumber],
    [PhoneNumberConfirmed],
    [TwoFactorEnabled],
    [LockoutEndDateUtc],
    [LockoutEnabled],
    [AccessFailedCount],
    [UserName]
)
VALUES (
    NEWID(), -- Genera un nuevo ID único
    'Juan Pablo',
    'Aguiar Maroto',
    30,
    'Guanacaste',
    '11-43456789',
    'https://lh3.googleusercontent.com/HIt2iEXOtDWPd58HyHFRN50FHrvo3ZERnTfVPMxrQ2912i8IkKrRzBDjA9i2tYo65t5qJqf_GrmUTaHUr1Egj20xjYP8BHNHWVxubxKFL-qE7uW8wEsuqhejuyKm5XjszQ=w1280', 
    'JAguilar01CentrointegralSD@gmail.com',
    1, 
    '123456JAgui+', -- Sustituye con el hash real
    NEWID(), -- SecurityStamp (puedes generar uno)
    '1234567890',
    1, -- PhoneNumber confirmado
    0, -- TwoFactorEnabled
    NULL, -- LockoutEndDateUtc (NULL si no tiene bloqueo)
    0, -- LockoutEnabled (1 = true, 0 = false)
    0, -- AccessFailedCount
    'Juan.Pablo' -- Nombre de usuario
);

INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT u.Id, r.Id
FROM AspNetUsers u, AspNetRoles r
WHERE u.Email = 'JAguilar01CentrointegralSD@gmail.com' 
AND r.Name = 'Medico'

--Rol Contador
INSERT INTO [CentroIntegralSD].[dbo].[AspNetUsers] (
    [Id],
    [Nombre],
    [Apellido],
    [Edad_Paciente],
    [Direccion],
    [Cedula],
    [Imagen],
    [Email],
    [EmailConfirmed],
    [PasswordHash],
    [SecurityStamp],
    [PhoneNumber],
    [PhoneNumberConfirmed],
    [TwoFactorEnabled],
    [LockoutEndDateUtc],
    [LockoutEnabled],
    [AccessFailedCount],
    [UserName]
)
VALUES (
    NEWID(), 
    'Andrés',
    'Camacho Ulate',
    30,
    'Guanacaste',
    '11-43336789',
    'https://plus.unsplash.com/premium_photo-1661627681947-4431c8c97659?fm=jpg&q=60&w=3000&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MXx8aG9tYnJlJTIwZXNwYSVDMyVCMW9sfGVufDB8fDB8fHww', 
    'ACamacho01CentrointegralSD@gmail.com',
    1, 
    '123456JACama+', 
    NEWID(), 
    '1234567890',
    1, 
    0, 
    NULL, 
    0, 
    0, 
    'Andrés' 
);

INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT u.Id, r.Id
FROM AspNetUsers u, AspNetRoles r
WHERE u.Email = 'ACamacho01CentrointegralSD@gmail.com' 
AND r.Name = 'Contador'

--Rol Auditor
INSERT INTO [CentroIntegralSD].[dbo].[AspNetUsers] (
    [Id],
    [Nombre],
    [Apellido],
    [Edad_Paciente],
    [Direccion],
    [Cedula],
    [Imagen],
    [Email],
    [EmailConfirmed],
    [PasswordHash],
    [SecurityStamp],
    [PhoneNumber],
    [PhoneNumberConfirmed],
    [TwoFactorEnabled],
    [LockoutEndDateUtc],
    [LockoutEnabled],
    [AccessFailedCount],
    [UserName]
)
VALUES (
    NEWID(), 
    'Ashely Nicole',
    'Chavarria Hidalgo',
    29,
    'Guanacaste, cañas',
    '11-43459039',
    'https://images.ecestaticos.com/eBvaltTEE03goqkr6_DLbrNOQxw=/484x270:5299x3536/992x700/filters:fill(white):format(jpg)/f.elconfidencial.com%2Foriginal%2Fb0d%2F0c8%2F01d%2Fb0d0c801d29a3fc13d9ec30ef361d0cf.jpg', 
    'AChavarria01CentrointegralSD@gmail.com',
    1, 
    '123456ACha+',
    NEWID(), 
    '1234567890',
    1, 
    0,
    NULL, 
    0, 
    0, 
    'Ashely.Nicole' 
);

INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT u.Id, r.Id
FROM AspNetUsers u, AspNetRoles r
WHERE u.Email = 'AChavarria01CentrointegralSD@gmail.com' 
AND r.Name = 'Auditor'

--Rol Administrador
INSERT INTO [CentroIntegralSD].[dbo].[AspNetUsers] (
    [Id],
    [Nombre],
    [Apellido],
    [Edad_Paciente],
    [Direccion],
    [Cedula],
    [Imagen],
    [Email],
    [EmailConfirmed],
    [PasswordHash],
    [SecurityStamp],
    [PhoneNumber],
    [PhoneNumberConfirmed],
    [TwoFactorEnabled],
    [LockoutEndDateUtc],
    [LockoutEnabled],
    [AccessFailedCount],
    [UserName]
)
VALUES (
    NEWID(), 
    'Carmen',
    'Ulate Arias',
    30,
    'Guanacaste',
    '11-43489789',
    'https://st3.depositphotos.com/12985790/15794/i/450/depositphotos_157947226-stock-photo-man-looking-at-camera.jpg', 
    'AUlate01CentrointegralSD@gmail.com',
    1, 
    '123456AUlat+', 
    NEWID(), 
    '1234567890',
    1, 
    0, 
    NULL,
    0, 
    0, 
    'Anderson.Adrian' 
);

INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT u.Id, r.Id
FROM AspNetUsers u, AspNetRoles r
WHERE u.Email = 'AUlate01CentrointegralSD@gmail.com' 
AND r.Name = 'Administrador'


--Diarios_Contables--

INSERT INTO [dbo].[Diarios_Contables]
([Id_Tipo_Registro], [Codigo_Diario], [Descripcion], [Activo], [Fecha_Creacion])
VALUES
(1, 'DVEN', 'Diario de Ventas', 1, GETDATE()),
(2, 'DCOM', 'Diario de Compras', 1, GETDATE()),
(3, 'DBAN', 'Diario de Bancos', 1, GETDATE()),
(4, 'DCAJ', 'Diario de Caja', 1, GETDATE()),
(5, 'DPRO', 'Diario de Provisiones', 1, GETDATE());


--Bancos--

INSERT INTO [dbo].[Bancos]
([Id_Diario], [Nombre_Banco], [Codigo_Banco])
VALUES
(1, 'Banco Nacional', 'BN001'),
(2, 'Banco de Costa Rica', 'BCR001'),
(3, 'BAC San José', 'BAC001'),
(4, 'Scotiabank', 'SCT001'),
(5, 'Banco Popular', 'BP001');


--Pagos--

INSERT INTO [dbo].[Pagos]
([Id_Banco], [Numero_Referencia], [Fecha_Pago], [Tipo_Pago], [Monto], 
[Beneficiario], [Cuenta_Beneficiario], [Estado_Pago], [Descripcion], 
[Observaciones], [Usuario_Registro], [Fecha_Registro], 
[Usuario_Modificacion], [Fecha_Modificacion])
VALUES
(1, 'REF001-2025', '2025-02-08 09:00:00', 'TRANSFERENCIA', 1500.00,
'Juan Pérez', '123456789', 'COMPLETADO', 'Pago de servicios profesionales',
'Factura #A001', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()),

(2, 'REF002-2025', '2025-02-08 10:15:00', 'CHEQUE', 2750.50,
'María Rodríguez', '987654321', 'PENDIENTE', 'Pago de materiales de oficina',
'Cheque #5001', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()),

(3, 'REF003-2025', '2025-02-08 11:30:00', 'TRANSFERENCIA', 3200.75,
'Carlos González', '456789123', 'COMPLETADO', 'Pago de mantenimiento',
'Servicio mensual', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()),

(4, 'CAJA001-2025', '2025-02-08 12:45:00', 'EFECTIVO', 500.00,
'Ana Martínez', 'N/A', 'COMPLETADO', 'Pago de viáticos',
'Viaje a sucursal', 'ADMIN', GETDATE(), 'ADMIN', GETDATE()),

(5, 'CAJA002-2025', '2025-02-08 13:15:00', 'EFECTIVO', 750.25,
'Pedro Sánchez', 'N/A', 'COMPLETADO', 'Reembolso de gastos',
'Facturas adjuntas', 'ADMIN', GETDATE(), 'ADMIN', GETDATE());

--Conciliaciones_Bancarias--

INSERT INTO [dbo].[Conciliaciones_Bancarias]
([Id_Banco], [Id_Diario], [Id_Tipo_Registro], [Fecha_Conciliacion], 
[Saldo_Contable], [Saldo_Banco])
VALUES

(1, 1, 1, '2025-01-31 23:59:59', 25000.00, 25150.75),
(2, 2, 2, '2025-01-31 23:59:59', 35000.00, 34850.25),
(3, 3, 3, '2025-01-31 23:59:59', 42000.00, 42150.50),
(4, 4, 4, '2025-01-31 23:59:59', 31000.00, 30925.75),
(5, 5, 5, '2025-01-31 23:59:59', 28000.00, 28175.25);


--Empleado--

INSERT INTO [dbo].[Empleado]
([Id_Estado], [Comentarios], [Nombre], [Apellido], [Cedula], 
[Correo], [Jornada], [Fecha_registro], [Departamento])
VALUES
(1, 'Empleado del área de desarrollo', 'Carlos', 'Rodríguez', '1-1234-5678',
'carlos.rodriguez@empresa.com', 'Tiempo Completo', GETDATE(), 'Tecnología'),

(2, 'Empleado del área contable', 'María', 'González', '2-3456-7890',
'maria.gonzalez@empresa.com', 'Medio Tiempo', GETDATE(), 'Contabilidad');


--Pagos_Diarios--

INSERT INTO [dbo].[Pagos_Diarios]
([Id_Contabilidad], [Id_Empleado], [Fecha_Pago], [Monto], 
[Metodo_Pago], [Estado_Pago], [Observaciones], [Fecha_Registro])
VALUES
(1, 1, '2025-02-08 14:30:00', 45000.00,
'Transferencia', 'Completado', 'Pago diario por servicios prestados', GETDATE()),

(2, 2, '2025-02-08 14:45:00', 25000.00,
'Transferencia', 'Completado', 'Pago diario medio tiempo', GETDATE());


--Movimientos_Bancarios--

INSERT INTO [dbo].[Movimientos_Bancarios]
([Id_Diario], [Id_Conciliacion], [Id_Pagos_Diarios], [Ingresos], [Egresos], 
[Saldo], [Fecha_Movimiento], [Descripcion])
VALUES
(1, 1, 3, 10000, 45000, 155000.00, '2025-02-08 14:30:00',
'Pago diario a empleado Carlos Rodríguez - Tiempo Completo'),

(2, 2, 4, 8000, 25000, 130000.00, '2025-02-08 14:45:00',
'Pago diario a empleado María González - Medio Tiempo');


--PagosXNomina--

INSERT INTO [dbo].[PagosXNomina]
([Id_Contabilidad], [Id_Pago], [Id_Empleado], [Fecha_Pago], 
[Monto_Pago], [Metodo_Pago], [Estado_Pago], [Observaciones], [Fecha_Registro])
VALUES
(1, 1, 1, '2025-02-15 00:00:00', 
950000.00, 'Transferencia Bancaria', 'Programado', 
'Pago de nómina primera quincena febrero 2025 - Tiempo Completo', GETDATE()),

(2, 2, 2, '2025-02-15 00:00:00', 
475000.00, 'Transferencia Bancaria', 'Programado', 
'Pago de nómina primera quincena febrero 2025 - Medio Tiempo', GETDATE());

--Empleado--

INSERT INTO [CentroIntegralSD].[dbo].[Empleado] 
    ([Id_Estado], [Comentarios], [Nombre], [Apellido], [Cedula], [Correo], [Jornada], [Fecha_registro], [Departamento])
VALUES
    ( 1, 'Empresa', 'Juan', 'Pérez', '001-1234567-0', 'juan.perez@email.com', 'Completa', GETDATE(), 'Proveedor'),
    ( 1, 'Empresa', 'María', 'Gómez', '002-7654321-0', 'maria.gomez@email.com', 'Medio Tiempo', GETDATE(), 'Proveedor'),
    ( 1, 'Empresa', 'Carlos', 'Rodríguez', '003-1122334-0', 'carlos.rodriguez@email.com', 'Completa', GETDATE(), 'Proveedor');


--Movimientos_Bancarios--

INSERT INTO [dbo].[Movimientos_Bancarios]
([Id_Diario], [Id_Conciliacion], [Id_Pago], [Id_Banco], [Ingresos], [Egresos], [Saldo], [Fecha_Movimiento], [Descripcion])
VALUES
(1, 1, 1, 1, 5000, 0, 5000.00, '2025-02-10 09:00:00', 'Depósito inicial cuenta corriente');

INSERT INTO [dbo].[Movimientos_Bancarios]
([Id_Diario], [Id_Conciliacion], [Id_Pago], [Id_Banco], [Ingresos], [Egresos], [Saldo], [Fecha_Movimiento], [Descripcion])
VALUES
(2, 2, 2, 2, 0, 1500, 3500.00, '2025-02-10 10:30:00', 'Pago servicios básicos');

INSERT INTO [dbo].[Movimientos_Bancarios]
([Id_Diario], [Id_Conciliacion], [Id_Pago], [Id_Banco], [Ingresos], [Egresos], [Saldo], [Fecha_Movimiento], [Descripcion])
VALUES
(3, 3, 3, 3, 2000, 0, 5500.00, '2025-02-10 14:15:00', 'Cobro factura #A-123');


--Tipos Regsitros Adicionales--

INSERT INTO [dbo].[Tipo_Registro] ([Nombre], [Descripcion])
VALUES ('Gastos de suministro medicos', 'Registro de gastos relacionados con la compra de medicamentos y fármacos');

INSERT INTO [dbo].[Tipo_Registro] ([Nombre], [Descripcion])
VALUES ('Gastos de suministro medicos', 'Registro de gastos de insumos y materiales para procedimientos quirúrgicos');

INSERT INTO [dbo].[Tipo_Registro] ([Nombre], [Descripcion])
VALUES ('Gastos de suministro medicos', 'Registro de gastos en compra y mantenimiento de equipos médicos');

INSERT INTO [dbo].[Tipo_Registro] ([Nombre], [Descripcion])
VALUES ('Gastos de suministro medicos', 'Registro de gastos en materiales de uso diario hospitalario como guantes, jeringas, vendas');

--Cambio de fotos *No es necesario cambiarlos*
UPDATE [CentroIntegralSD].[dbo].[AspNetUsers]
SET Imagen = 'https://images.pexels.com/photos/13870864/pexels-photo-13870864.jpeg?auto=compress&cs=tinysrgb&w=600&lazy=load'
WHERE Id = '26f1be6c-b0a8-48da-99ae-40944d8a6dca';