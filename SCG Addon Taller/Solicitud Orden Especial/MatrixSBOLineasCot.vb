Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrixSBOLineasCot : Inherits MatrixSBO

    Public Sub New(ByVal uniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(uniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#Region "Declaraciones"
    Private _columnaItemCode As ColumnaMatrixSBOEditText(Of String)
    Private _columnaDescripcion As ColumnaMatrixSBOEditText(Of String)
    Private _columnaPorcDesc As ColumnaMatrixSBOEditText(Of String)
    Private _columnaMoneda As ColumnaMatrixSBOEditText(Of String)
    Private _columnaPrecio As ColumnaMatrixSBOCheckBox(Of String)
    Private _columnaComentarios As ColumnaMatrixSBOEditText(Of String)
    Private _columnaTax As ColumnaMatrixSBOEditText(Of String)
    Private _columnaIdRxOrd As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCosto As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCantidad As ColumnaMatrixSBOEditText(Of String)
    Private _columnaSeleccionar As ColumnaMatrixSBOCheckBox(Of String)
    Private _columnaCol_CPen As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCol_CSol As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCol_CRec As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCol_CPDe As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCol_CPTr As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCol_CPBo As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCol_Compra As ColumnaMatrixSBOEditText(Of String)

#End Region

#Region "Propiedades"
    Public Property ColumnaItemCode() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaItemCode
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaItemCode = value
        End Set
    End Property

    Public Property ColumnaDescripcion() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaDescripcion
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaDescripcion = value
        End Set
    End Property

    Public Property ColumnaPorcDesc() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaPorcDesc
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaPorcDesc = value
        End Set
    End Property

    Public Property ColumnaMoneda() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaMoneda
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaMoneda = value
        End Set
    End Property

    Public Property ColumnaPrecio() As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaPrecio
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
            _columnaPrecio = value
        End Set
    End Property

    Public Property ColumnaComentarios() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaComentarios
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaComentarios = value
        End Set
    End Property

    Public Property ColumnaTax() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaTax
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaTax = value
        End Set
    End Property

    Public Property ColumnaIdRxOrd() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaIdRxOrd
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaIdRxOrd = value
        End Set
    End Property

    Public Property ColumnaCosto() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCosto
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCosto = value
        End Set
    End Property

    Public Property ColumnaCantidad() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCantidad
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCantidad = value
        End Set
    End Property

    Public Property ColumnaSeleccionar() As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaSeleccionar
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
            _columnaSeleccionar = value
        End Set
    End Property

    Public Property ColumnaColCPen() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_CPen
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_CPen = value
        End Set
    End Property

    Public Property ColumnaColCSol() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_CSol
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_CSol = value
        End Set
    End Property

    Public Property ColumnaColCRec() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_CRec
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_CRec = value
        End Set
    End Property

    Public Property ColumnaColCPDe() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_CPDe
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_CPDe = value
        End Set
    End Property

    Public Property ColumnaColCPTr() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_CPTr
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_CPTr = value
        End Set
    End Property

    Public Property ColumnaColCPBo() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_CPBo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_CPBo = value
        End Set
    End Property

    Public Property ColumnaColCompra() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Compra
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Compra = value
        End Set
    End Property

#End Region

    Public Overrides Sub CreaColumnas()

        ColumnaItemCode = New ColumnaMatrixSBOEditText(Of String)("colItemCo", True, "colItemCo", Me)
        ColumnaDescripcion = New ColumnaMatrixSBOEditText(Of String)("colDesc", True, "colDesc", Me)
        ColumnaCantidad = New ColumnaMatrixSBOEditText(Of String)("colQtn", True, "colQtn", Me)
        ColumnaPorcDesc = New ColumnaMatrixSBOEditText(Of String)("colPorDs", True, "colPorDs", Me)
        ColumnaMoneda = New ColumnaMatrixSBOEditText(Of String)("colMoned", True, "colMoned", Me)
        ColumnaPrecio = New ColumnaMatrixSBOCheckBox(Of String)("colPrec", True, "colPrec", Me)
        ColumnaComentarios = New ColumnaMatrixSBOEditText(Of String)("colComen", True, "colComen", Me)
        ColumnaTax = New ColumnaMatrixSBOEditText(Of String)("colTaxCd", True, "colTaxCd", Me)
        ColumnaIdRxOrd = New ColumnaMatrixSBOEditText(Of String)("colIdRXO", True, "colIdRXO", Me)
        ColumnaCosto = New ColumnaMatrixSBOEditText(Of String)("colCosto", True, "colCosto", Me)
        ColumnaSeleccionar = New ColumnaMatrixSBOCheckBox(Of String)("col_Sel", True, "col_Sel", Me)
        ColumnaColCPen = New ColumnaMatrixSBOEditText(Of Decimal)("col_CPend", True, "col_CPend", Me)
        ColumnaColCSol = New ColumnaMatrixSBOEditText(Of Decimal)("col_CSol", True, "col_CSol", Me)
        ColumnaColCRec = New ColumnaMatrixSBOEditText(Of Decimal)("col_CRec", True, "col_CRec", Me)
        ColumnaColCPDe = New ColumnaMatrixSBOEditText(Of Decimal)("col_PenDev", True, "col_PenDev", Me)
        ColumnaColCPTr = New ColumnaMatrixSBOEditText(Of Decimal)("col_PenTra", True, "col_PenTra", Me)
        ColumnaColCPBo = New ColumnaMatrixSBOEditText(Of Decimal)("col_PenBod", True, "col_PenBod", Me)
        ColumnaColCompra = New ColumnaMatrixSBOEditText(Of String)("col_Compra", True, "col_Compra", Me)

    End Sub

    Public Overrides Sub LigaColumnas()

        ColumnaItemCode.AsignaBindingDataTable()
        ColumnaDescripcion.AsignaBindingDataTable()
        ColumnaCantidad.AsignaBindingDataTable()
        ColumnaPorcDesc.AsignaBindingDataTable()
        ColumnaMoneda.AsignaBindingDataTable()
        ColumnaPrecio.AsignaBindingDataTable()
        ColumnaComentarios.AsignaBindingDataTable()
        ColumnaTax.AsignaBindingDataTable()
        ColumnaIdRxOrd.AsignaBindingDataTable()
        ColumnaCosto.AsignaBindingDataTable()
        ColumnaSeleccionar.AsignaBindingDataTable()
        ColumnaColCPen.AsignaBindingDataTable()
        ColumnaColCSol.AsignaBindingDataTable()
        ColumnaColCRec.AsignaBindingDataTable()
        ColumnaColCPDe.AsignaBindingDataTable()
        ColumnaColCPTr.AsignaBindingDataTable()
        ColumnaColCPBo.AsignaBindingDataTable()
        ColumnaColCompra.AsignaBindingDataTable()

    End Sub

End Class
