   
//    function SelectAllChildNodes() 
//    {
//        //debugger;
//        var obj = window.event.srcElement; 
//        var treeNodeFound = false;

//        var checkedState; 
//        if (obj.tagName == "INPUT" && obj.type == "checkbox") 
//        {
//            var treeNode = obj; 
//            checkedState = treeNode.checked;
//            do
//            {
//                obj = obj.parentElement;
//            } while (obj.tagName != "TABLE") 
//            
//            var parentTreeLevel = obj.rows[0].cells.length;            
//            var parentTreeNode = obj.rows[0].cells[0]; 
//            var tables = obj.parentElement.getElementsByTagName("TABLE");
//            var numTables = tables.length;
//            if (numTables >= 1) 
//            {
//                for (iCount=0; iCount < numTables; iCount++) 
//                {
//                    if (tables[iCount] == obj) 
//                    {
//                        treeNodeFound = true; 
//                        iCount++;
//                        if (iCount == numTables) 
//                        {
//                            return; 
//                        }
//                    }
//                    if (treeNodeFound == true) 
//                    {
//                        var childTreeLevel = tables[iCount].rows[0].cells.length;
//                        if (childTreeLevel > parentTreeLevel) 
//                        {
//                            var cell = tables[iCount].rows[0].cells[childTreeLevel - 1];
//                            var inputs = cell.getElementsByTagName("INPUT"); 
//                            inputs[0].checked = checkedState;
//                        }
//                        else
//                        {
//                            return; 
//                        }
//                    }
//                }
//            }
//        }
//    }

///////////////////////////////////////////////////////////////
  
///////////////////////////////////////////////////////////////////////
function OnTreeClick(evt) {
    var src = window.event != window.undefined ? window.event.srcElement : evt.target;
    var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
    if (isChkBoxClick) {
        var parentTable = GetParentByTagName("table", src);
        var nxtSibling = parentTable.nextSibling;
        //check if nxt sibling is not null & is an element node
        if (nxtSibling && nxtSibling.nodeType == 1) {
            if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
            {
                //check or uncheck children at all levels
                CheckUncheckChildren(parentTable.nextSibling, src.checked);
            }
        }
        //check or uncheck parents at all levels
        CheckUncheckParents(src, src.checked);
    }
}

function CheckUncheckChildren(childContainer, check) {
    var childChkBoxes = childContainer.getElementsByTagName("input");
    var childChkBoxCount = childChkBoxes.length;
    for (var i = 0; i < childChkBoxCount; i++) {
        childChkBoxes[i].checked = check;
    }
}

function CheckUncheckParents(srcChild, check) {
    var parentDiv = GetParentByTagName("div", srcChild);
    var parentNodeTable = parentDiv.previousSibling;
    if (parentNodeTable) {
        var checkUncheckSwitch;
        if (check) //checkbox checked
        {
            var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
           //codigo original
          //  if (isAllSiblingsChecked)
               checkUncheckSwitch = true;
            //else
              // return; //do not need to check parent if any(one or more) child not checked
            //modificado por thuertas
            
        }
        else //checkbox unchecked
        {   //modificado por thuertas
            if (AreAllSiblingsUnChecked(srcChild))
                checkUncheckSwitch = false;
            else
                checkUncheckSwitch = true;
            
        }

        var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
        if (inpElemsInParentTable.length > 0) {
            var parentNodeChkBox = inpElemsInParentTable[0];
            
          
            parentNodeChkBox.checked = checkUncheckSwitch;
            //do the same recursively
            CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
        }
    }
}

function AreAllSiblingsChecked(chkBox) {
    var parentDiv = GetParentByTagName("div", chkBox);
    var childCount = parentDiv.childNodes.length;
    for (var i = 0; i < childCount; i++) {
        if (parentDiv.childNodes[i].nodeType == 1) {
            //check if the child node is an element node
            if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                //if any of sibling nodes are not checked, return false
                if (!prevChkBox.checked) {
                  
                    return false;
                   
                }
            }
        }
    }
    return true;
}

//creado por thuertas
//valida si todos los siblings estan unchecked
function AreAllSiblingsUnChecked(chkBox) {
    var parentDiv = GetParentByTagName("div", chkBox);
    var childCount = parentDiv.childNodes.length;
    for (var i = 0; i < childCount; i++) {
        if (parentDiv.childNodes[i].nodeType == 1) {
            //check if the child node is an element node
            if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                //if any of sibling nodes are not checked, return false
                if (prevChkBox.checked) {

                    return false;

                }
            }
        }
    }
    return true;
}

//utility function to get the container of an element by tagname
function GetParentByTagName(parentTagName, childElementObj) {
    var parent = childElementObj.parentNode;
    while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
        parent = parent.parentNode;
    }
    return parent;
}