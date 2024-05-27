//only make ajax call when page is loaded
$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {url: '/admin/product/getall'},  
        "columns": [
            { data: 'title', title: "Title" },
            { data: 'isbn', title:"ISBN"},
            { data: 'description', title:"Description" },
            { data: 'author', title:"Author" },
            { data: 'category.name' , title:"Category"},
            { data: 'publisher', title:"Publisher" },
            { data: 'listPrice', title: "Price" },
            {
                data: 'id', title: "", render: function (data) {
                    return `                  <div class="w-75 btn-group" role="group">
                      <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2">
                               <i class="bi bi-pencil-square"></i>   Edit
                      </a>
                      
                     <a href="/admin/product/delete?id=${data}" class="btn btn-danger mx-2">
                         <i class="bi bi-trash2-fill"></i>   Delete
                     </a>
                          
                     <a href="/admin/product/delete/id=${data}" class="btn btn-danger mx-2 ">Delete Now</a>
                  </div>`}
                },
        ]
     });


    
}
