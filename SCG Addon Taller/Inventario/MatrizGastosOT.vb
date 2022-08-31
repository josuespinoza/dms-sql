
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizGastosOT : Inherits MatrixSBO
#Region "Declaraciones"

    'Columnas de la matriz
    Private _columnaCol_Cod As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Des As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Can As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Mon As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Pre As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Apr As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Sel As ColumnaMatrixSBOCheckBox(Of String)
    Private _columnaCol_Per As ColumnaMatrixSBOCheckBox(Of String)
    Private _columnaCol_Asi As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Fac As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Imp As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Lnum As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Cos As ColumnaMatrixSBOEditText(Of String)

#End Region

#Region "Propiedades"
    'propiedades para las columnas de la matriz

    Public Property ColumnaColCod As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Cod
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Cod = value
        End Set
    End Property

    Public Property ColumnaColDes As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Des
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Des = value
        End Set
    End Property

    Public Property ColumnaColCan As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Can
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Can = value
        End Set
    End Property

    Public Property ColumnaColMon As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Mon
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Mon = value
        End Set
    End Property

    Public Property ColumnaColPre As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Pre
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Pre = value
        End Set
    End Property

    Public Property ColumnaColCos As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Cos
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Cos = value
        End Set
    End Property

    Public Property ColumnaColApr As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Apr
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Apr = value
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

    Public Property ColumnaColPer As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaCol_Per
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
            _columnaCol_Per = value
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

    Public Property ColumnaColFac As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Fac
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Fac = value
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

    Public Property ColumnaColLnum As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Lnum
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Lnum = value
        End Set
    End Property


#End Region

#Region "Constructor"

    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#End Region

#Region "Metodos"

    ''' <summary>
    ''' Crea las columnas para la matriz
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub CreaColumnas()
        ColumnaColSel = New ColumnaMatrixSBOCheckBox(Of String)("Col_sel", True, "sel", Me)
        ColumnaColper = New ColumnaMatrixSBOCheckBox(Of String)("Col_per", True, "per", Me)
        ColumnaColCod = New ColumnaMatrixSBOEditText(Of String)("Col_cod", True, "cod", Me)
        ColumnaColDes = New ColumnaMatrixSBOEditText(Of String)("Col_des", True, "des", Me)
        ColumnaColCan = New ColumnaMatrixSBOEditText(Of String)("Col_can", True, "can", Me)
        ColumnaColMon = New ColumnaMatrixSBOEditText(Of String)("Col_mon", True, "mon", Me)
        ColumnaColPre = New ColumnaMatrixSBOEditText(Of String)("Col_pre", True, "pre", Me)
        ColumnaColCos = New ColumnaMatrixSBOEditText(Of String)("Col_cos", True, "cos", Me)
        ColumnaColApr = New ColumnaMatrixSBOEditText(Of String)("Col_apr", True, "apr", Me)
        ColumnaColAsi = New ColumnaMatrixSBOEditText(Of String)("Col_Asi", True, "asi", Me)
        ColumnaColFac = New ColumnaMatrixSBOEditText(Of String)("Col_Fac", True, "fac", Me)
        ColumnaColImp = New ColumnaMatrixSBOEditText(Of String)("Col_imp", True, "imp", Me)
        ColumnaColLnum = New ColumnaMatrixSBOEditText(Of String)("Col_lnum", True, "lnum", Me)

    End Sub

    ''' <summary>
    ''' Liga las columnas de la matriz
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub LigaColumnas()

        ColumnaColSel.AsignaBindingDataTable()
        ColumnaColPer.AsignaBindingDataTable()
        ColumnaColCod.AsignaBindingDataTable()
        ColumnaColDes.AsignaBindingDataTable()
        ColumnaColCan.AsignaBindingDataTable()
        ColumnaColMon.AsignaBindingDataTable()
        ColumnaColPre.AsignaBindingDataTable()
        ColumnaColApr.AsignaBindingDataTable()
        ColumnaColAsi.AsignaBindingDataTable()
        ColumnaColFac.AsignaBindingDataTable()
        ColumnaColImp.AsignaBindingDataTable()
        ColumnaColCos.AsignaBindingDataTable()
        ColumnaColLnum.AsignaBindingDataTable()

    End Sub

#End Region

End Class


