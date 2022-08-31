Imports SAPbouiCOM
Imports SCG.SBOFramework.UI


Public Class MatrizEntradaVehiculos : Inherits MatrixSBO


    Private _columnaCol_ID As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Cod As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Mar As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Est As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Mod As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Vin As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Mot As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Ubi As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Sta As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Tip As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Ano As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Col As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Ref As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Art As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Ped As ColumnaMatrixSBOEditText(Of String)

    Private _columnaCol_Cos As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCol_Asi As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Ent As ColumnaMatrixSBOEditText(Of String)

    Private _columnaCol_DMar As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_DEst As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_DMod As ColumnaMatrixSBOEditText(Of String)


    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#Region "Propiedades"

    Public Property ColumnaColID As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_ID
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_ID = value
        End Set
    End Property

    Public Property ColumnaColCod As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Cod
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Cod = value
        End Set
    End Property

    Public Property ColumnaColMar As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Mar
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Mar = value
        End Set
    End Property

    Public Property ColumnaColEst As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Est
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Est = value
        End Set
    End Property

    Public Property ColumnaColMod As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Mod
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Mod = value
        End Set
    End Property

    Public Property ColumnaColVin As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Vin
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Vin = value
        End Set
    End Property

    Public Property ColumnaColMot As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Mot
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Mot = value
        End Set
    End Property

    Public Property ColumnaColUbi As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Ubi
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Ubi = value
        End Set
    End Property

    Public Property ColumnaColSta As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Sta
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Sta = value
        End Set
    End Property

    Public Property ColumnaColAno As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Ano
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Ano = value
        End Set
    End Property

    Public Property ColumnaColTip As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Tip
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Tip = value
        End Set
    End Property

    Public Property ColumnaColCol As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Col
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Col = value
        End Set
    End Property

    Public Property ColumnaColRef As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Ref
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Ref = value
        End Set
    End Property

    Public Property ColumnaColArt As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Art
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Art = value
        End Set
    End Property

    Public Property ColumnaColPed As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Ped
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Ped = value
        End Set
    End Property

    Public Property ColumnaColCos As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_Cos
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_Cos = value
        End Set
    End Property


    Public Property ColumnaColAsi As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Asi
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Asi = value
        End Set
    End Property

    Public Property ColumnaColEnt As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Ent
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Ent = value
        End Set
    End Property

    Public Property ColumnaColDesMar As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_DMar
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_DMar = value
        End Set
    End Property

    Public Property ColumnaColDesEst As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_DEst
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_DEst = value
        End Set
    End Property

    Public Property ColumnaColDesMod As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_DMod
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_DMod = value
        End Set
    End Property


#End Region

    Public Overrides Sub CreaColumnas()

        ColumnaColPed = New ColumnaMatrixSBOEditText(Of String)("col_Ped", True, "U_Num_Ped", Me)
        ColumnaColCod = New ColumnaMatrixSBOEditText(Of String)("col_Unid", True, "U_Cod_Uni", Me)
        ColumnaColMar = New ColumnaMatrixSBOEditText(Of String)("col_Marc", True, "U_Cod_Mar", Me)
        ColumnaColEst = New ColumnaMatrixSBOEditText(Of String)("col_Esti", True, "U_Cod_Est", Me)
        ColumnaColMod = New ColumnaMatrixSBOEditText(Of String)("col_Mode", True, "U_Cod_Mod", Me)
        ColumnaColVin = New ColumnaMatrixSBOEditText(Of String)("col_Vin", True, "U_Num_VIN", Me)
        ColumnaColMot = New ColumnaMatrixSBOEditText(Of String)("col_Mot", True, "U_Num_Mot", Me)
        ColumnaColUbi = New ColumnaMatrixSBOEditText(Of String)("col_Ubic", True, "U_Cod_Ubi", Me)
        ColumnaColSta = New ColumnaMatrixSBOEditText(Of String)("col_Esta", True, "U_Estado", Me)
        ColumnaColAno = New ColumnaMatrixSBOEditText(Of String)("col_Ano", True, "U_Ano_Veh", Me)
        ColumnaColTip = New ColumnaMatrixSBOEditText(Of String)("col_Tipo", True, "U_Cod_Tip", Me)
        ColumnaColCol = New ColumnaMatrixSBOEditText(Of String)("col_Col", True, "U_Cod_Col", Me)
        ColumnaColRef = New ColumnaMatrixSBOEditText(Of String)("col_Ref", True, "U_Line_Ref", Me)
        ColumnaColArt = New ColumnaMatrixSBOEditText(Of String)("col_Art", True, "U_Cod_Art", Me)
        ColumnaColID = New ColumnaMatrixSBOEditText(Of String)("col_ID", True, "U_ID_Veh", Me)
        ColumnaColCos = New ColumnaMatrixSBOEditText(Of Decimal)("col_Cos", True, "U_Monto_Gr", Me)
        ColumnaColAsi = New ColumnaMatrixSBOEditText(Of String)("col_Asi", True, "U_Num_Asiento", Me)
        ColumnaColEnt = New ColumnaMatrixSBOEditText(Of String)("col_GR", True, "U_Num_Entrada", Me)

        ColumnaColDesMar = New ColumnaMatrixSBOEditText(Of String)("col_DMar", True, "U_Des_Mar", Me)
        ColumnaColDesEst = New ColumnaMatrixSBOEditText(Of String)("col_DEst", True, "U_Des_Est", Me)
        ColumnaColDesMod = New ColumnaMatrixSBOEditText(Of String)("col_DMod", True, "U_Des_Mod", Me)

    End Sub


    Public Overrides Sub LigaColumnas()

    End Sub

End Class


