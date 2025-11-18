# APEC WS Publicos

## .NET
### Version
`8.0.404`
### Scripts
**Build Script**
> `dotnet build`


## Mandato
### Desarrolle un UDDI de Servicios Web que cumpla con las siguientes caracteristicas (debe ):

Exponga un Servicio Web para:
- Consultar la tasa de cambio segun la moneda indicada ✔
- Consultar el índice de inflación del período ✔
- Consultar la salud Financiera de un cliente ✔
- Consultar el historial crediticio de un cliente ✔
- Consulta de Uso de Web Services (Filtros: Nombre WS, Rango de fecha de invocación) ✔

#### Solucion

##### Solution Function
```
APEC.WSPublicos.API
    Program.cs
    CustomFaultExceptionTransformer.cs
    Controllers
        TasaCambioController.cs

APEC.WSPublicos.Aplicacion
    Interfaces
        IHistorialCrediticioServiceSoap.cs
        IInflacionServiceSoap.cs
        IReporteUsoServiceSoap.cs
        ISaludFinancieraServiceSoap.cs
        ITasaCambioServiceSoap.cs
    Servicios
        HistorialCrediticioServiceSoap.cs
        InflacionServiceSoap.cs
        ReporteUsoServiceSoap.cs
        SaludFinancieraServiceSoap.cs
        TasaCambioService.cs
        TasaCambioServiceSoap .cs

APEC.WSPublicos.Infrastructura
    Data
        AppDbContext.cs
    Modelos
        Cliente.cs
        HistorialCrediticio.cs
        IndiceInflacion.cs
        RegistroUsoServicio.cs
        TasaCambiaria.cs
```

#### Ejemplos request de consumo
##### Ejemplos TasaCambioServiceSoap request de consumo

```
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:ObtenerTasa>
         <!--Optional:-->
         <tem:codigoMoneda>USD</tem:codigoMoneda>
      </tem:ObtenerTasa>
   </soapenv:Body>
</soapenv:Envelope>
```

##### Ejemplos InflacionServiceSoap request de consumo

```
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:ObtenerIndiceInflacion>
         <!--Optional:-->
         <tem:periodo>202505</tem:periodo>
      </tem:ObtenerIndiceInflacion>
   </soapenv:Body>
</soapenv:Envelope>
```

##### Ejemplos SaludFinancieraServiceSoap request de consumo

```
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:ConsultarSaludFinanciera>
         <!--Optional:-->
         <tem:cedulaRnc>001-1234567-8</tem:cedulaRnc>
      </tem:ConsultarSaludFinanciera>
   </soapenv:Body>
</soapenv:Envelope>
```
##### Ejemplos HistorialCrediticioServiceSoap request de consumo

```
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:ConsultarHistorial>
         <!--Optional:-->
         <tem:cedulaRnc>001-1234567-8</tem:cedulaRnc>
      </tem:ConsultarHistorial>
   </soapenv:Body>
</soapenv:Envelope>
```

##### Ejemplos ReporteUsoServiceSoap request de consumo

```
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:ConsultarUsoServicios>
         <!--Optional:-->
         <tem:nombreServicio>HistorialCrediticio</tem:nombreServicio>
         <tem:fechaInicio>2025-06-04T00:00:00</tem:fechaInicio>
         <tem:fechaFin>2025-07-10T00:00:00</tem:fechaFin>
      </tem:ConsultarUsoServicios>
   </soapenv:Body>
</soapenv:Envelope>
```

Opciones para Reporte de uso:

```
"TasaCambio"
"Inflacion"
"SaludFinanciera"
"HistorialCrediticio"
```



  
