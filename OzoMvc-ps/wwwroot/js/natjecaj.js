
$(document).on('click', '.deleterow', function () {
    event.preventDefault();
    var tr = $(this).parents("tr");
    tr.remove();

    clearOldMessage();
});

$(function () {  

    $("#natjecaj-dodaj").click(function () {
        event.preventDefault();
        DodajNatjecaj();
    });   
});

function DodajNatjecaj() {
    var sifra = $("#ponudjivac-id").val();
    if (sifra != '') {
        if ($("[name='Stavke[" + sifra + "].PonudjivacId").length > 0) {
            alert('Ponuđivač je već u natječaju');
            return;
        }
       
        var ponuda = parseFloat($("#natjecaj-ponuda").val().replace(',', '.')); 
        if (isNaN(ponuda))
            ponuda = 0;     
            

            var naziv = $('#ponudjivac-naziv').val();
            var dobiven = $('#natjecaj-dobiven').val();
            var template = $('#template').html();
            var rijec=$("#natjecaj-dobiven option:selected").text();
            if (dobiven==null){ 
            dobiven = false;  
            var selected1="selected";
            }
            else{
                if($("#natjecaj-dobiven option:selected").text().trim()=="Da"){
                    var selected2="selected"; 
                    var selected1=""; 
                }
                else{
                    var selected1="selected"; 
                    var selected2=""; 
                }
            }
          
          
        
           
        template = template.replace(/--sifra--/g, sifra)
                            .replace(/--ponuda--/g, ponuda)
                            .replace(/--dobiven--/g, dobiven)
                            .replace(/--naziv--/g, naziv)
                            .replace(/--selected1--/g, selected1)
                            .replace(/--selected2--/g, selected2)
                           
                            
        $(template).find('tr').insertBefore($("#table-stavke").find('tr').last());

        $("#ponudjivac-id").val('');
        $("#ponudjivac-naziv").val('');
        $("#natjecaj-ponuda").val('');
        $("#natjecaj-dobiven").val('');
        

        
        $("#tempmessage").siblings().remove();
        $("#tempmessage").removeClass("alert-success");
        $("#tempmessage").removeClass("alert-danger");
        $("#tempmessage").html('');
    }
}