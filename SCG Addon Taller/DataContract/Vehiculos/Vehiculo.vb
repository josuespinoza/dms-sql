Imports System.Collections.Generic

Public Class Vehiculo

    Private _Canceled As String
    Public Property Canceled() As String
        Get
            Return _Canceled
        End Get
        Set(value As String)
            _Canceled = value
        End Set
    End Property


    Private _Transfered As String
    Public Property Transfered() As String
        Get
            Return _Transfered
        End Get
        Set(value As String)
            _Transfered = value
        End Set
    End Property


    Private _DataSource As String
    Public Property DataSource() As String
        Get
            Return _DataSource
        End Get
        Set(value As String)
            _DataSource = value
        End Set
    End Property


    Private _U_TIPINV As String
    Public Property U_TIPINV() As String
        Get
            Return _U_TIPINV
        End Get
        Set(value As String)
            _U_TIPINV = value
        End Set
    End Property


    Private _U_FCHRES As DateTime
    Public Property U_FCHRES() As DateTime
        Get
            Return _U_FCHRES
        End Get
        Set(value As DateTime)
            _U_FCHRES = value
        End Set
    End Property


    Private _U_FECFINR As DateTime
    Public Property U_FECFINR() As DateTime
        Get
            Return _U_FECFINR
        End Get
        Set(value As DateTime)
            _U_FECFINR = value
        End Set
    End Property


    Private _UpdateDate As DateTime
    Public Property UpdateDate() As DateTime
        Get
            Return _UpdateDate
        End Get
        Set(value As DateTime)
            _UpdateDate = value
        End Set
    End Property


    Private _U_FechaVen As DateTime
    Public Property U_FechaVen() As DateTime
        Get
            Return _U_FechaVen
        End Get
        Set(value As DateTime)
            _U_FechaVen = value
        End Set
    End Property


    Private _CreateDate As DateTime
    Public Property CreateDate() As DateTime
        Get
            Return _CreateDate
        End Get
        Set(value As DateTime)
            _CreateDate = value
        End Set
    End Property


    Private _U_FCHINV As DateTime
    Public Property U_FCHINV() As DateTime
        Get
            Return _U_FCHINV
        End Get
        Set(value As DateTime)
            _U_FCHINV = value
        End Set
    End Property


    Private _U_FchUSv As DateTime
    Public Property U_FchUSv() As DateTime
        Get
            Return _U_FchUSv
        End Get
        Set(value As DateTime)
            _U_FchUSv = value
        End Set
    End Property


    Private _U_FchPrSv As DateTime
    Public Property U_FchPrSv() As DateTime
        Get
            Return _U_FchPrSv
        End Get
        Set(value As DateTime)
            _U_FchPrSv = value
        End Set
    End Property


    Private _U_FchRsva As DateTime
    Public Property U_FchRsva() As DateTime
        Get
            Return _U_FchRsva
        End Get
        Set(value As DateTime)
            _U_FchRsva = value
        End Set
    End Property


    Private _U_FchVcRva As DateTime
    Public Property U_FchVcRva() As DateTime
        Get
            Return _U_FchVcRva
        End Get
        Set(value As DateTime)
            _U_FchVcRva = value
        End Set
    End Property


    Private _U_Fha_Ing_Inv As DateTime
    Public Property U_Fha_Ing_Inv() As DateTime
        Get
            Return _U_Fha_Ing_Inv
        End Get
        Set(value As DateTime)
            _U_Fha_Ing_Inv = value
        End Set
    End Property


    Private _U_GaranIni As DateTime
    Public Property U_GaranIni() As DateTime
        Get
            Return _U_GaranIni
        End Get
        Set(value As DateTime)
            _U_GaranIni = value
        End Set
    End Property


    Private _U_GaranFin As DateTime
    Public Property U_GaranFin() As DateTime
        Get
            Return _U_GaranFin
        End Get
        Set(value As DateTime)
            _U_GaranFin = value
        End Set
    End Property


    Private _U_HorSer As Int32
    Public Property U_HorSer() As Int32
        Get
            Return _U_HorSer
        End Get
        Set(value As Int32)
            _U_HorSer = value
        End Set
    End Property


    Private _U_NUMFAC As Int32
    Public Property U_NUMFAC() As Int32
        Get
            Return _U_NUMFAC
        End Get
        Set(value As Int32)
            _U_NUMFAC = value
        End Set
    End Property


    Private _DocEntry As Int32
    Public Property DocEntry() As Int32
        Get
            Return _DocEntry
        End Get
        Set(value As Int32)
            _DocEntry = value
        End Set
    End Property


    Private _U_FrecSvc As Int32
    Public Property U_FrecSvc() As Int32
        Get
            Return _U_FrecSvc
        End Get
        Set(value As Int32)
            _U_FrecSvc = value
        End Set
    End Property


    Private _LogInst As Int32
    Public Property LogInst() As Int32
        Get
            Return _LogInst
        End Get
        Set(value As Int32)
            _LogInst = value
        End Set
    End Property


    Private _UserSign As Int32
    Public Property UserSign() As Int32
        Get
            Return _UserSign
        End Get
        Set(value As Int32)
            _UserSign = value
        End Set
    End Property


    Private _U_Num_Cili As Int32
    Public Property U_Num_Cili() As Int32
        Get
            Return _U_Num_Cili
        End Get
        Set(value As Int32)
            _U_Num_Cili = value
        End Set
    End Property


    Private _U_CTOVTA As Int32
    Public Property U_CTOVTA() As Int32
        Get
            Return _U_CTOVTA
        End Get
        Set(value As Int32)
            _U_CTOVTA = value
        End Set
    End Property


    Private _U_GarantKM As Int32
    Public Property U_GarantKM() As Int32
        Get
            Return _U_GarantKM
        End Get
        Set(value As Int32)
            _U_GarantKM = value
        End Set
    End Property


    Private _U_Potencia As Int32
    Public Property U_Potencia() As Int32
        Get
            Return _U_Potencia
        End Get
        Set(value As Int32)
            _U_Potencia = value
        End Set
    End Property


    Private _U_Peso As Int32
    Public Property U_Peso() As Int32
        Get
            Return _U_Peso
        End Get
        Set(value As Int32)
            _U_Peso = value
        End Set
    End Property


    Private _U_Cilindra As Int32
    Public Property U_Cilindra() As Int32
        Get
            Return _U_Cilindra
        End Get
        Set(value As Int32)
            _U_Cilindra = value
        End Set
    End Property


    Private _U_Dispo As Int32
    Public Property U_Dispo() As Int32
        Get
            Return _U_Dispo
        End Get
        Set(value As Int32)
            _U_Dispo = value
        End Set
    End Property


    Private _U_SALINID As Double
    Public Property U_SALINID() As Double
        Get
            Return _U_SALINID
        End Get
        Set(value As Double)
            _U_SALINID = value
        End Set
    End Property


    Private _U_SALINIC As Double
    Public Property U_SALINIC() As Double
        Get
            Return _U_SALINIC
        End Get
        Set(value As Double)
            _U_SALINIC = value
        End Set
    End Property


    Private _U_FLELOC As Double
    Public Property U_FLELOC() As Double
        Get
            Return _U_FLELOC
        End Get
        Set(value As Double)
            _U_FLELOC = value
        End Set
    End Property


    Private _U_TIPCAM As Double
    Public Property U_TIPCAM() As Double
        Get
            Return _U_TIPCAM
        End Get
        Set(value As Double)
            _U_TIPCAM = value
        End Set
    End Property


    Private _U_COSINV As Double
    Public Property U_COSINV() As Double
        Get
            Return _U_COSINV
        End Get
        Set(value As Double)
            _U_COSINV = value
        End Set
    End Property


    Private _U_VALHAC As Double
    Public Property U_VALHAC() As Double
        Get
            Return _U_VALHAC
        End Get
        Set(value As Double)
            _U_VALHAC = value
        End Set
    End Property


    Private _U_GASTRA As Double
    Public Property U_GASTRA() As Double
        Get
            Return _U_GASTRA
        End Get
        Set(value As Double)
            _U_GASTRA = value
        End Set
    End Property


    Private _U_VTADOL As Double
    Public Property U_VTADOL() As Double
        Get
            Return _U_VTADOL
        End Get
        Set(value As Double)
            _U_VTADOL = value
        End Set
    End Property


    Private _U_VTACOL As Double
    Public Property U_VTACOL() As Double
        Get
            Return _U_VTACOL
        End Get
        Set(value As Double)
            _U_VTACOL = value
        End Set
    End Property


    Private _U_ValorNet As Double
    Public Property U_ValorNet() As Double
        Get
            Return _U_ValorNet
        End Get
        Set(value As Double)
            _U_ValorNet = value
        End Set
    End Property


    Private _U_Precio As Double
    Public Property U_Precio() As Double
        Get
            Return _U_Precio
        End Get
        Set(value As Double)
            _U_Precio = value
        End Set
    End Property


    Private _U_CosPro As Double
    Public Property U_CosPro() As Double
        Get
            Return _U_CosPro
        End Get
        Set(value As Double)
            _U_CosPro = value
        End Set
    End Property


    Private _U_TCRSalIni As Double
    Public Property U_TCRSalIni() As Double
        Get
            Return _U_TCRSalIni
        End Get
        Set(value As Double)
            _U_TCRSalIni = value
        End Set
    End Property


    Private _U_Km_Unid As Double
    Public Property U_Km_Unid() As Double
        Get
            Return _U_Km_Unid
        End Get
        Set(value As Double)
            _U_Km_Unid = value
        End Set
    End Property


    Private _U_Bono As Double
    Public Property U_Bono() As Double
        Get
            Return _U_Bono
        End Get
        Set(value As Double)
            _U_Bono = value
        End Set
    End Property


    Private _U_Cod_Prov As String
    Public Property U_Cod_Prov() As String
        Get
            Return _U_Cod_Prov
        End Get
        Set(value As String)
            _U_Cod_Prov = value
        End Set
    End Property


    Private _U_Nom_Prov As String
    Public Property U_Nom_Prov() As String
        Get
            Return _U_Nom_Prov
        End Get
        Set(value As String)
            _U_Nom_Prov = value
        End Set
    End Property


    Private _U_ContratoV As String
    Public Property U_ContratoV() As String
        Get
            Return _U_ContratoV
        End Get
        Set(value As String)
            _U_ContratoV = value
        End Set
    End Property


    Private _U_DocPedido As String
    Public Property U_DocPedido() As String
        Get
            Return _U_DocPedido
        End Get
        Set(value As String)
            _U_DocPedido = value
        End Set
    End Property


    Private _U_Cod_Tec As String
    Public Property U_Cod_Tec() As String
        Get
            Return _U_Cod_Tec
        End Get
        Set(value As String)
            _U_Cod_Tec = value
        End Set
    End Property


    Private _U_Consig As String
    Public Property U_Consig() As String
        Get
            Return _U_Consig
        End Get
        Set(value As String)
            _U_Consig = value
        End Set
    End Property


    Private _U_Moneda As String
    Public Property U_Moneda() As String
        Get
            Return _U_Moneda
        End Get
        Set(value As String)
            _U_Moneda = value
        End Set
    End Property


    Private _U_DocRecepcion As String
    Public Property U_DocRecepcion() As String
        Get
            Return _U_DocRecepcion
        End Get
        Set(value As String)
            _U_DocRecepcion = value
        End Set
    End Property


    Private _U_Comentarios As String
    Public Property U_Comentarios() As String
        Get
            Return _U_Comentarios
        End Get
        Set(value As String)
            _U_Comentarios = value
        End Set
    End Property


    Private _U_CCar As String
    Public Property U_CCar() As String
        Get
            Return _U_CCar
        End Get
        Set(value As String)
            _U_CCar = value
        End Set
    End Property


    Private _U_Pote As String
    Public Property U_Pote() As String
        Get
            Return _U_Pote
        End Get
        Set(value As String)
            _U_Pote = value
        End Set
    End Property


    Private _U_DiEje As String
    Public Property U_DiEje() As String
        Get
            Return _U_DiEje
        End Get
        Set(value As String)
            _U_DiEje = value
        End Set
    End Property


    Private _U_Ramv As String
    Public Property U_Ramv() As String
        Get
            Return _U_Ramv
        End Get
        Set(value As String)
            _U_Ramv = value
        End Set
    End Property


    Private _U_Cant_Eje As String
    Public Property U_Cant_Eje() As String
        Get
            Return _U_Cant_Eje
        End Get
        Set(value As String)
            _U_Cant_Eje = value
        End Set
    End Property


    Private _U_NoPedFb As String
    Public Property U_NoPedFb() As String
        Get
            Return _U_NoPedFb
        End Get
        Set(value As String)
            _U_NoPedFb = value
        End Set
    End Property


    Private _U_ArtVentDesc As String
    Public Property U_ArtVentDesc() As String
        Get
            Return _U_ArtVentDesc
        End Get
        Set(value As String)
            _U_ArtVentDesc = value
        End Set
    End Property


    Private _U_Des_Col_Tap As String
    Public Property U_Des_Col_Tap() As String
        Get
            Return _U_Des_Col_Tap
        End Get
        Set(value As String)
            _U_Des_Col_Tap = value
        End Set
    End Property


    Private _U_Clasificacion As String
    Public Property U_Clasificacion() As String
        Get
            Return _U_Clasificacion
        End Get
        Set(value As String)
            _U_Clasificacion = value
        End Set
    End Property


    Private _U_Estado_Nuevo As String
    Public Property U_Estado_Nuevo() As String
        Get
            Return _U_Estado_Nuevo
        End Get
        Set(value As String)
            _U_Estado_Nuevo = value
        End Set
    End Property


    Private _U_fechaSync As String
    Public Property U_fechaSync() As String
        Get
            Return _U_fechaSync
        End Get
        Set(value As String)
            _U_fechaSync = value
        End Set
    End Property


    Private _U_ArtVent As String
    Public Property U_ArtVent() As String
        Get
            Return _U_ArtVent
        End Get
        Set(value As String)
            _U_ArtVent = value
        End Set
    End Property


    Private _U_Cli_Ven As String
    Public Property U_Cli_Ven() As String
        Get
            Return _U_Cli_Ven
        End Get
        Set(value As String)
            _U_Cli_Ven = value
        End Set
    End Property


    Private _U_Tipo_Reing As String
    Public Property U_Tipo_Reing() As String
        Get
            Return _U_Tipo_Reing
        End Get
        Set(value As String)
            _U_Tipo_Reing = value
        End Set
    End Property


    Private _U_ClNo_Ven As String
    Public Property U_ClNo_Ven() As String
        Get
            Return _U_ClNo_Ven
        End Get
        Set(value As String)
            _U_ClNo_Ven = value
        End Set
    End Property


    Private _U_CardCode As String
    Public Property U_CardCode() As String
        Get
            Return _U_CardCode
        End Get
        Set(value As String)
            _U_CardCode = value
        End Set
    End Property


    Private _U_CardName As String
    Public Property U_CardName() As String
        Get
            Return _U_CardName
        End Get
        Set(value As String)
            _U_CardName = value
        End Set
    End Property


    Private _U_Categori As String
    Public Property U_Categori() As String
        Get
            Return _U_Categori
        End Get
        Set(value As String)
            _U_Categori = value
        End Set
    End Property


    Private _U_Combusti As String
    Public Property U_Combusti() As String
        Get
            Return _U_Combusti
        End Get
        Set(value As String)
            _U_Combusti = value
        End Set
    End Property


    Private _U_Tip_Cabi As String
    Public Property U_Tip_Cabi() As String
        Get
            Return _U_Tip_Cabi
        End Get
        Set(value As String)
            _U_Tip_Cabi = value
        End Set
    End Property


    Private _U_Transmis As String
    Public Property U_Transmis() As String
        Get
            Return _U_Transmis
        End Get
        Set(value As String)
            _U_Transmis = value
        End Set
    End Property


    Private _U_Accesori As String
    Public Property U_Accesori() As String
        Get
            Return _U_Accesori
        End Get
        Set(value As String)
            _U_Accesori = value
        End Set
    End Property


    Private _U_VENRES As String
    Public Property U_VENRES() As String
        Get
            Return _U_VENRES
        End Get
        Set(value As String)
            _U_VENRES = value
        End Set
    End Property


    Private _U_Cod_Fab As String
    Public Property U_Cod_Fab() As String
        Get
            Return _U_Cod_Fab
        End Get
        Set(value As String)
            _U_Cod_Fab = value
        End Set
    End Property


    Private _U_Tipo_Ven As String
    Public Property U_Tipo_Ven() As String
        Get
            Return _U_Tipo_Ven
        End Get
        Set(value As String)
            _U_Tipo_Ven = value
        End Set
    End Property


    Private _U_OBSRES As String
    Public Property U_OBSRES() As String
        Get
            Return _U_OBSRES
        End Get
        Set(value As String)
            _U_OBSRES = value
        End Set
    End Property


    Private _U_ARREST As String
    Public Property U_ARREST() As String
        Get
            Return _U_ARREST
        End Get
        Set(value As String)
            _U_ARREST = value
        End Set
    End Property


    Private _U_TipTecho As String
    Public Property U_TipTecho() As String
        Get
            Return _U_TipTecho
        End Get
        Set(value As String)
            _U_TipTecho = value
        End Set
    End Property


    Private _U_Carrocer As String
    Public Property U_Carrocer() As String
        Get
            Return _U_Carrocer
        End Get
        Set(value As String)
            _U_Carrocer = value
        End Set
    End Property


    Private _U_Cod_Ubic As String
    Public Property U_Cod_Ubic() As String
        Get
            Return _U_Cod_Ubic
        End Get
        Set(value As String)
            _U_Cod_Ubic = value
        End Set
    End Property


    Private _U_Tipo As String
    Public Property U_Tipo() As String
        Get
            Return _U_Tipo
        End Get
        Set(value As String)
            _U_Tipo = value
        End Set
    End Property


    Private _U_Estatus As String
    Public Property U_Estatus() As String
        Get
            Return _U_Estatus
        End Get
        Set(value As String)
            _U_Estatus = value
        End Set
    End Property


    Private _U_Tipo_Tra As String
    Public Property U_Tipo_Tra() As String
        Get
            Return _U_Tipo_Tra
        End Get
        Set(value As String)
            _U_Tipo_Tra = value
        End Set
    End Property


    Private _U_Num_Plac As String
    Public Property U_Num_Plac() As String
        Get
            Return _U_Num_Plac
        End Get
        Set(value As String)
            _U_Num_Plac = value
        End Set
    End Property


    Private _U_Cod_Col As String
    Public Property U_Cod_Col() As String
        Get
            Return _U_Cod_Col
        End Get
        Set(value As String)
            _U_Cod_Col = value
        End Set
    End Property


    Private _U_Des_Col As String
    Public Property U_Des_Col() As String
        Get
            Return _U_Des_Col
        End Get
        Set(value As String)
            _U_Des_Col = value
        End Set
    End Property


    Private _U_ColorTap As String
    Public Property U_ColorTap() As String
        Get
            Return _U_ColorTap
        End Get
        Set(value As String)
            _U_ColorTap = value
        End Set
    End Property


    Private _U_Num_VIN As String
    Public Property U_Num_VIN() As String
        Get
            Return _U_Num_VIN
        End Get
        Set(value As String)
            _U_Num_VIN = value
        End Set
    End Property


    Private _U_Num_Mot As String
    Public Property U_Num_Mot() As String
        Get
            Return _U_Num_Mot
        End Get
        Set(value As String)
            _U_Num_Mot = value
        End Set
    End Property


    Private _U_MarcaMot As String
    Public Property U_MarcaMot() As String
        Get
            Return _U_MarcaMot
        End Get
        Set(value As String)
            _U_MarcaMot = value
        End Set
    End Property


    Private _Code As String
    Public Property Code() As String
        Get
            Return _Code
        End Get
        Set(value As String)
            _Code = value
        End Set
    End Property


    Private _Name As String
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(value As String)
            _Name = value
        End Set
    End Property


    Private _U_Cod_Unid As String
    Public Property U_Cod_Unid() As String
        Get
            Return _U_Cod_Unid
        End Get
        Set(value As String)
            _U_Cod_Unid = value
        End Set
    End Property


    Private _U_Cod_Marc As String
    Public Property U_Cod_Marc() As String
        Get
            Return _U_Cod_Marc
        End Get
        Set(value As String)
            _U_Cod_Marc = value
        End Set
    End Property


    Private _U_Des_Marc As String
    Public Property U_Des_Marc() As String
        Get
            Return _U_Des_Marc
        End Get
        Set(value As String)
            _U_Des_Marc = value
        End Set
    End Property


    Private _U_Cod_Mode As String
    Public Property U_Cod_Mode() As String
        Get
            Return _U_Cod_Mode
        End Get
        Set(value As String)
            _U_Cod_Mode = value
        End Set
    End Property


    Private _U_Des_Mode As String
    Public Property U_Des_Mode() As String
        Get
            Return _U_Des_Mode
        End Get
        Set(value As String)
            _U_Des_Mode = value
        End Set
    End Property


    Private _U_Cod_Esti As String
    Public Property U_Cod_Esti() As String
        Get
            Return _U_Cod_Esti
        End Get
        Set(value As String)
            _U_Cod_Esti = value
        End Set
    End Property


    Private _U_Des_Esti As String
    Public Property U_Des_Esti() As String
        Get
            Return _U_Des_Esti
        End Get
        Set(value As String)
            _U_Des_Esti = value
        End Set
    End Property


    Private _U_Ano_Vehi As Int16
    Public Property U_Ano_Vehi() As Int16
        Get
            Return _U_Ano_Vehi
        End Get
        Set(value As Int16)
            _U_Ano_Vehi = value
        End Set
    End Property


    Private _CreateTime As Int16
    Public Property CreateTime() As Int16
        Get
            Return _CreateTime
        End Get
        Set(value As Int16)
            _CreateTime = value
        End Set
    End Property


    Private _U_Cant_Pas As Int16
    Public Property U_Cant_Pas() As Int16
        Get
            Return _U_Cant_Pas
        End Get
        Set(value As Int16)
            _U_Cant_Pas = value
        End Set
    End Property


    Private _U_CantPuer As Int16
    Public Property U_CantPuer() As Int16
        Get
            Return _U_CantPuer
        End Get
        Set(value As Int16)
            _U_CantPuer = value
        End Set
    End Property


    Private _UpdateTime As Int16
    Public Property UpdateTime() As Int16
        Get
            Return _UpdateTime
        End Get
        Set(value As Int16)
            _UpdateTime = value
        End Set
    End Property


    Private _U_GarantTM As Int16
    Public Property U_GarantTM() As Int16
        Get
            Return _U_GarantTM
        End Get
        Set(value As Int16)
            _U_GarantTM = value
        End Set
    End Property

    Private _AccesoriosxVehiculo As List(Of AccesoriosxVehiculo)
    Public Property AccesoriosxVehiculo As List(Of AccesoriosxVehiculo)
        Get
            Return _AccesoriosxVehiculo
        End Get
        Set(value As List(Of AccesoriosxVehiculo))
            _AccesoriosxVehiculo = value
        End Set
    End Property

    Private _BonosXVehiculo As List(Of BonosXVehiculo)
    Public Property BonosXVehiculo As List(Of BonosXVehiculo)
        Get
            Return _BonosXVehiculo
        End Get
        Set(value As List(Of BonosXVehiculo))
            _BonosXVehiculo = value
        End Set
    End Property

    Private _TrazabilizadXVehiculo As List(Of TrazabilizadXVehiculo)
    Public Property TrazabilizadXVehiculo As List(Of TrazabilizadXVehiculo)
        Get
            Return _TrazabilizadXVehiculo
        End Get
        Set(value As List(Of TrazabilizadXVehiculo))
            _TrazabilizadXVehiculo = value
        End Set
    End Property

End Class

