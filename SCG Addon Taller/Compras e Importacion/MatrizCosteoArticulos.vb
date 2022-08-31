Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizCosteoArticulos : Inherits MatrixSBO

    Private _columnaCol_ID As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Cod As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Mar As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Est As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Mod As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Vin As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Ano As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Tot As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Imp As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Ped As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Sel As ColumnaMatrixSBOCheckBox(Of String)
    Private _columnaCol_Art As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Tra As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Ent As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Col As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Mot As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Cta As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_TImp As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Ref As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_IDVeh As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Fac As ColumnaMatrixSBOEditText(Of Integer)


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

    Public Property ColumnaColIDVeh As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_IDVeh
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_IDVeh = value
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

    Public Property ColumnaColTot As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Tot
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Tot = value
        End Set
    End Property

    Public Property ColumnaColImp As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Imp
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Imp = value
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

    Public Property ColumnaColArt As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Art
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Art = value
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

    Public Property ColumnaColAno As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Ano
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Ano = value
        End Set
    End Property

    Public Property ColumnaColSel As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaCol_Sel
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
            _columnaCol_Sel = value
        End Set
    End Property

    Public Property ColumnaColTra As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Tra
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Tra = value
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

    Public Property ColumnaColMot As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Mot
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Mot = value
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

    Public Property ColumnaColCta As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Cta
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Cta = value
        End Set
    End Property

    Public Property ColumnaColTImp As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_TImp
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_TImp = value
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

    Public Property ColumnaColFac As ColumnaMatrixSBOEditText(Of Integer)
        Get
            Return _columnaCol_Fac
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Integer))
            _columnaCol_Fac = value
        End Set
    End Property


#End Region

    Public Overrides Sub CreaColumnas()

        ColumnaColPed = New ColumnaMatrixSBOEditText(Of String)("col_Ped", True, "U_Cod_Pedido", Me)
        ColumnaColEnt = New ColumnaMatrixSBOEditText(Of String)("col_Ent", True, "U_Cod_Entrada", Me)
        ColumnaColCod = New ColumnaMatrixSBOEditText(Of String)("col_Cod", True, "U_Cod_Unid", Me)
        ColumnaColVin = New ColumnaMatrixSBOEditText(Of String)("col_Vin", True, "U_Num_VIN", Me)
        ColumnaColMot = New ColumnaMatrixSBOEditText(Of String)("col_Mot", True, "U_Num_Motor", Me)
        ColumnaColMar = New ColumnaMatrixSBOEditText(Of String)("col_Mar", True, "U_Cod_Marca", Me)
        ColumnaColEst = New ColumnaMatrixSBOEditText(Of String)("col_Est", True, "U_Cod_Estilo", Me)
        ColumnaColMod = New ColumnaMatrixSBOEditText(Of String)("col_Mod", True, "U_Cod_Modelo", Me)
        ColumnaColCol = New ColumnaMatrixSBOEditText(Of String)("col_Col", True, "U_Cod_Color", Me)
        ColumnaColImp = New ColumnaMatrixSBOEditText(Of String)("col_Imp", True, "U_Cod_Impuesto", Me)
        ColumnaColTra = New ColumnaMatrixSBOEditText(Of String)("col_Tra", True, "U_Cod_Trasacc", Me)
        ColumnaColCta = New ColumnaMatrixSBOEditText(Of String)("col_Cta", True, "U_Num_Cta", Me)
        ColumnaColTot = New ColumnaMatrixSBOEditText(Of String)("col_Tot", True, "U_Mnt_Total", Me)
        ColumnaColTImp = New ColumnaMatrixSBOEditText(Of String)("col_TImp", True, "U_Mnt_Imp", Me)
        ColumnaColRef = New ColumnaMatrixSBOEditText(Of String)("col_Ref", True, "U_Line_Ref", Me)
        ColumnaColArt = New ColumnaMatrixSBOEditText(Of String)("col_Art", True, "U_Cod_Art", Me)
        ColumnaColAno = New ColumnaMatrixSBOEditText(Of String)("col_Ano", True, "U_Ano_Veh", Me)
        ColumnaColIDVeh = New ColumnaMatrixSBOEditText(Of String)("col_IDVeh", True, "U_ID_Unid", Me)
        ColumnaColFac = New ColumnaMatrixSBOEditText(Of Integer)("col_Fac", True, "U_NumFactura", Me)
    End Sub


    Public Overrides Sub LigaColumnas()

        ColumnaColCod.AsignaBindingDataTable()
        ColumnaColMar.AsignaBindingDataTable()
        ColumnaColEst.AsignaBindingDataTable()
        ColumnaColMod.AsignaBindingDataTable()
        ColumnaColTot.AsignaBindingDataTable()
        ColumnaColImp.AsignaBindingDataTable()
        ColumnaColVin.AsignaBindingDataTable()
        ColumnaColAno.AsignaBindingDataTable()
        ColumnaColSel.AsignaBindingDataTable()
        ColumnaColPed.AsignaBindingDataTable()
        ColumnaColArt.AsignaBindingDataTable()
        ColumnaColTra.AsignaBindingDataTable()
        ColumnaColEnt.AsignaBindingDataTable()
        ColumnaColFac.AsignaBindingDataTable()
        
    End Sub

End Class


