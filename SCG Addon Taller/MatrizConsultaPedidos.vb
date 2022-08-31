Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Public Class MatrizConsultaPedidos : Inherits MatrixSBO

#Region "Declaraciones"

    Private _columnaCol_Code As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Desc As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_QtyP As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_QtyR As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Year As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Color As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_FechaArr As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Transmision As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Traccion As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Combustible As ColumnaMatrixSBOEditText(Of String)
#End Region

#Region "Propiedades"


    Public Property ColumnaColCode As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Code
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Code = value
        End Set
    End Property

    Public Property ColumnaColDesc As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Desc
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Desc = value
        End Set
    End Property

    Public Property ColumnaColQuantityP As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_QtyP
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_QtyP = value
        End Set
    End Property

    Public Property ColumnaColQuantityR As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_QtyR
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_QtyR = value
        End Set
    End Property

    Public Property ColumnaColYear As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Year
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Year = value
        End Set
    End Property

    Public Property ColumnaColColor As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Color
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Color = value
        End Set
    End Property

    Public Property ColumnaColFechaArr As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_FechaArr
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_FechaArr = value
        End Set
    End Property

    Public Property ColumnaColTransmision As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Transmision
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Transmision = value
        End Set
    End Property

    Public Property ColumnaColTraccion As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Traccion
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Traccion = value
        End Set
    End Property

    Public Property ColumnaColCombustible As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Combustible
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Combustible = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#End Region

#Region "Métodos"

    Public Overrides Sub CreaColumnas()
        ColumnaColCode = New ColumnaMatrixSBOEditText(Of String)("col_code", True, "col_code", Me)
        ColumnaColDesc = New ColumnaMatrixSBOEditText(Of String)("col_desc", True, "col_desc", Me)
        ColumnaColYear = New ColumnaMatrixSBOEditText(Of String)("col_year", True, "col_year", Me)
        ColumnaColColor = New ColumnaMatrixSBOEditText(Of String)("col_color", True, "col_color", Me)
        ColumnaColQuantityP = New ColumnaMatrixSBOEditText(Of String)("col_qtyP", True, "col_qtyP", Me)
        ColumnaColQuantityR = New ColumnaMatrixSBOEditText(Of String)("col_qtyR", True, "col_qtyR", Me)
        ColumnaColFechaArr = New ColumnaMatrixSBOEditText(Of String)("col_feca", True, "col_feca", Me)
        ColumnaColTraccion = New ColumnaMatrixSBOEditText(Of String)("col_trac", True, "col_trac", Me)
        ColumnaColTransmision = New ColumnaMatrixSBOEditText(Of String)("col_tran", True, "col_tran", Me)
        ColumnaColCombustible = New ColumnaMatrixSBOEditText(Of String)("col_comb", True, "col_comb", Me)

    End Sub

    Public Overrides Sub LigaColumnas()

        ColumnaColCode.AsignaBindingDataTable()
        ColumnaColDesc.AsignaBindingDataTable()
        ColumnaColQuantityP.AsignaBindingDataTable()
        ColumnaColQuantityR.AsignaBindingDataTable()
        ColumnaColYear.AsignaBindingDataTable()
        ColumnaColColor.AsignaBindingDataTable()
        ColumnaColFechaArr.AsignaBindingDataTable()
        ColumnaColTraccion.AsignaBindingDataTable()
        ColumnaColTransmision.AsignaBindingDataTable()
        ColumnaColCombustible.AsignaBindingDataTable()

    End Sub

#End Region
End Class
