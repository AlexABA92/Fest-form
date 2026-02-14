// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.debugger
$(document).ready(
    function () {
      
       

        $(".back").click(() => {
            sessionStorage.setItem("return", "");
        })
        $("#modalCheck").change(() => {
            if ($("#modalCheck").is(":checked"))
                $("#modalBtn").prop("disabled", false)
            else
                $("#modalBtn").prop("disabled", true)
        })
        $(document).on("change", ".ConcertmasterNullCheckBox", (event) => {
            id = $(event.currentTarget).data("id")
            if ($(event.currentTarget).is(":checked")) {
                
                $(`#Performances_${id}_ConcertmasterPersonLastName`).val("")
                $(`#Performances_${id}_ConcertmasterPersonLastName`).prop("disabled", true)

                $(`#Performances_${id}_ConcertmasterPersonName`).val("")
                $(`#Performances_${id}_ConcertmasterPersonName`).prop("disabled", true)

                if ($(`#Performances_${id}_ConcertmasterPersonFatherName`).length) {
                    $(`#Performances_${id}_ConcertmasterPersonFatherName`).val("")
                    $(`#Performances_${id}_ConcertmasterPersonFatherName`).prop("disabled", true)
                }
            } else {
                $(`#Performances_${id}_ConcertmasterPersonLastName`).prop("disabled", false)
                $(`#Performances_${id}_ConcertmasterPersonName`).prop("disabled", false)
                $(`#Performances_${id}_ConcertmasterPersonFatherName`).prop("disabled", false)
            }
        })
        $("#submitBtn").on("click",()=> {
            if ($("#regForm").valid()) {
               $("#ModalFileLoading").modal("show")
                $("#submitBtn").prop("disabled", true);
                setTimeout(() => {
                    $("#regForm").trigger("submit");
                },400)
                
                
            }else 
            $("#submitBtn").prop("disabled", false);
        });

        $("#performancesList").on("change", '.numSelect', function () {
            var id = $(this).data('id')
            $(`#personList${id}`).remove()
            var sOption = $(this).find("option:selected");
            var selectPer = Math.abs(sOption.val());

            // Проверяем значение
            if (selectPer <= 3)
                $(`#personListforNum${id}`).append(`<div id="personList${id}"></div>`)
            for (var i = 0; i < selectPer; i++)
                personblock(id, `#personList${id}`, `ParticipantsNameList.Person${i + 1}`, $('#personListforNum0').data('label'))
        })
        $('#performancesList').on('click', '.delete', function () {
           
            var id = $(this).data('id');

            if (id < num) {
                for (var i = num; i >= id; i--) {
                    removeBlock(i)
                    numDicrement();
                }
            } else {
                removeBlock(id);
                numDicrement();
            }
            function removeBlock(int) {
                var class1 = `.performance${int}`;
                $(class1).remove();
            }
            function numDicrement() {
                if (num > 0) {
                    num--
                }

            }
        });
        $('#option1').click(function () {
            if (!$('#option1').hasClass('active')) {
                $('#option2').removeClass('active')
                $('#option1').addClass('active')
                organizationFildToggle()
            }
        })
        $('#option2').click(function () {
            if (!$('#option2').hasClass('active')) {
                $('#option1').removeClass('active')
                $('#option2').addClass('active')
                organizationFildToggle()
            }
        })
        $("#Performances_0_ParticipantsNumber").change(function () {
            
            var sOption = $(this).find("option:selected");
            var num = Math.abs(sOption.val());  // Проверяем значение

            $('#par1').addClass("d-none");
            $('#par2').addClass("d-none");
            $('#par3').addClass("d-none");

            if (num == 1) {
                $('#par1').removeClass("d-none");
            }
            if (num == 2) {
                $('#par2').removeClass("d-none");
                $('#par1').removeClass("d-none");
            }
            if (num == 3) { 
                $('#par2').removeClass("d-none");
                $('#par1').removeClass("d-none");
                $('#par3').removeClass("d-none");
            }
        });
        $('#addP').click(function () {

            if (num >= 4) {
                $("#warningDialog").modal("show")
                return
            }

            function perNumberOption() {
                var optios;

                ParticipantsNumberList.forEach(function (item) {
                    optios += `<option value="${item.Id}">${$(`#perNum${item.Id}`).data('name')}</option>`
                })
                return optios
            }
            function genreOption() {
                var optios;

                genreList.forEach(function (item) {
                    optios += `<option value="${item.Id}">${$(`#genNum${item.Id}`).data('name')}</option>`
                })
                return optios
            }
            function ageGroupOption() {
                var optios;

                categoryList.forEach(function (item) {
                    optios += `<option value="${item.Id}">${$(`#ageGroup${item.Id}`).data('name')}</option>`
                })
                return optios
            }

            num++
            $('#performancesList').append(`<div id="per${num}" class="performance performance${num} border-top border-1 border-secondary m-1 p-1">

                    <div class="form-group  text-start mb-2">
                        <label asp-for="Performances[${num}].PerformanceName" class="control-label mb-2">${$('#perName').data('label')} :</label>
                        <input asp-for="Performances[${num}].PerformanceName" type="text" 
                        name="Performances[${num}].PerformanceName"
                        id="id="Performances_${num}_PerformanceName""
                        class="form-control fontSimple" aria-describedby="${$('#perName').data('label')
                }"/>
                        <span asp-validation-for="Performances[${num}].PerformanceName" class="text-danger fontError"></span>
                    </div>
                         `)
            personblock(num,`#per${num}`, "ChoreographerDirector", $('#CharLabel').data('label'))
            personblock(num,`#per${num}`, "Concertmaster", $('#ConcertmasterLabal').data('label'))
            $(`#per${num}`).append(`
            <div id="personListforNum${num}">
                <div class="container p-0 col-md-6">
                     <label for="parNum">${$('#PartNameListLabel').data('label')} : </label>
                     <select class="form-select text-center fontSimple fw-bold mt-1 numSelect"asp-for="Performances[${num}].ParticipantsNumberId"
                                data-id="${num}"
                                name="Performances[${num}].ParticipantsNumberId"
                                id="Performances_${num}_ParticipantsNumber">
                            <option value="" selected>------</option>
                            ${perNumberOption()}
                        </select>
                        <span asp-validation-for="Performances[${num}].ParticipantsNumberId" class="text-danger fontError"></span>
                </div>
                <div id="personList${num}"></div>
            </div>`)
            $(`#per${num}`).append(`
             <div class="row row-cols-sm-2 mt-2">
                    <div>
                        <label for="Performances_${num}_GenreId">${$('#genreLabel').data('label')} : </label>
                        <select class="form-select text-center fontSimple fw-bold mt-1" asp-for="Performances[${num}].GenreId"
                               id="Performances_${num}_Genre" aria-label="Floating label select example"
                                name="Performances[${num}].GenreId">
                            <option value="" selected>------</option>
                            ${genreOption()}
                        </select>
                        <span asp-validation-for="Performances[${num}].GenreId" class="text-danger fontError"></span>

                    </div>
                      <div >
                        <label for="Performances_${num}_PerformanceGroup">${$('#ageGroupLabel').data('label')} : </label>
                        <select class="form-select text-center fontSimple fw-bold mt-1" asp-for="Performances[${num}].CategoryId"
                                id="Performances_${num}_PerformanceGroup" aria-label="Floating label select example"
                                name="Performances[${num}].CategoryId">
                            <option value="" selected>------</option>
                           ${ageGroupOption()}
                        </select>
                        <span asp-validation-for="Performances[${num}].CategoryId" class="text-danger fontError"></span>
                     </div>
                </div>
            `)
            $(`#per${num}`).append(`
            <div class="row justify-content-around">
                    <div class="col-6">
                        <label for="time">${$('#timeLabel').data('label')}: </label>
                        <input type="time" asp-for="Performances[${num}].PerformanceTime"
                                name="Performances[${num}].PerformanceTime"
                               class="form-control col-12 col-md-3 text-center" id="time${num}" />
                        <span asp-validation-for="Performances[${num}].PerformanceTime" class="text-danger fontError"></span>
                    </div>
                    <div class="col-6 ">
                        <label for="startSpot">${$('#timeLabel').data('label')} : </label>
                        <div class="row justify-content-md-around justify-content-center">
                            <input type="radio" class="btn-check" asp-for="Performances[${num}].StartPoint"
                                   name="Performances[${num}].StartPoint" value="Wing" id="btn-check-outlined${num}" autocomplete="off" checked>
                            <label class="btn btn-outline-primary col-8 mb-1 col-md-4 fontSimple"
                                   for="btn-check-outlined${num}" >${$('#radio1').data('label')} </label>
                            <input type="radio" class="btn-check " asp-for="Performances[${num}].StartPoint"
                                   name="Performances[${num}].StartPoint" value="Point" id="btn-check-outlined${num}${num}" autocomplete="off">
                            <label class="btn btn-outline-primary col-8 mb-1 col-md-4 fontSimple"
                                   for="btn-check-outlined${num}${num}">${$('#radio2').data('label')} </label>
                        </div>
                    </div>
             </div>`)
              
            $(`#per${num}`).append(`
                <div class="form-group  text-start mb-2 container">
                    <label asp-for="Performances[${num}].YouTubeVideoURL" class="control-label  mb-2" id="teamName">${$('#youtubeLinc').data('label')}:</label>
                    <input asp-for="Performances[${num}].YouTubeVideoURL" name="Performances[${num}].YouTubeVideoURL" class="form-control fontSimple" aria-describedby="${$('#youtubeLinc').data('label')}" />
                    <span asp-validation-for="Performances[${num}].YouTubeVideoURL" class="text-danger  fontError"></span>
                </div>`)


            $(`#per${num}`).append(`
            <div class="text-end me-3">
                <button class="btn btn-danger delete  m-2" type="button" data-id="${num}"><i class="bi bi-trash"></i></button>
            </div>
            </div>`)

        })

        var personblock = function (index,id, model, label) {
            var pesonDiv = $(`<div class="form-group mb-3 text-start" id="perInput${id}"></div>`)

            if (model !== "Concertmaster")
                pesonDiv.append(labelReturn(label))
            else
                pesonDiv.append(checkboxDisable(label, num))

            pesonDiv.append(`                    
                        <div class="form-group row align-items-center">
                         <div class="col-12 col-md-4">
                            <input asp-for="Performances[${index}].${model}.PersonLastName"
                                   class="form-control fontSimple mb-1"
                                   name="Performances[${index}].${model}.PersonLastName"
                                   aria-describedby="Person Last Name"
                                   id="Performances_${num}_${model}PersonLastName"
                                   placeholder="${leng === "uk" ? "Призвище" : "Last Name"}" />
                            <span asp-validation-for="Performances[${index}].${model}.PersonLastName" class="text-danger fontError d-block"></span>
                        </div>
                        <div class="col-12 col-md-4">
                            <input asp-for="Performances[${index}].${model}.PersonName"
                                   name="Performances[${index}].${model}.PersonName"
                                   class="form-control fontSimple mb-1"
                                   aria-describedby="Person Name"
                                   id="Performances_${num}_${model}PersonName"
                                   placeholder="${leng === "uk" ? "Ім'я" :"Neme"}" />
                            <span asp-validation-for="Performances[${index}].${model}.PersonName" class="text-danger fontError d-block"></span>
                        </div>
                       
                        ${leng === "uk" ? addFatherName(index,model) : ""}
                        `)
            $(id).append(pesonDiv)

        }
        var labelReturn = (label) => {
            return `<label class="control-label mb-2 col-12 col-md-6">${label}  :</label>`
        }
        var checkboxDisable = function (label, num) {
            var div = $(`<div class="d-flex justify-content-between row"></div>`)
            div.append(labelReturn(label))
            div.append(`
                <div class="form-check mb-2 col-12 col-md-6 d-flex justify-content-center">
                <input class="form-check-input me-2 ConcertmasterNullCheckBox"
                    type="checkbox" value="" data-id="${num}""
                <label class="form-check-label" for="flexCheckDefault">
                    ${$('#checkLabel').data('label')}
                </label>
                </div >`)
            return div               
        }
       
        var erorsCheck = function () {
          
            for (var key in errorsJs) {
                if (errorsJs.hasOwnProperty(key)) {
                    //console.log("Field:", key);    // Field name (e.g., "Performances[1].PerformanceName")
                    // Access the array of errors for this field
                    errorsJs[key].forEach((errorMessage) => {
                        key = key.replace(/\[/g, '_') 
                            .replace(/\]/g, '_') 
                            .replace(/\./g, '')
                            id = "#" + key
                        var peer = $(id).parent()
                        $(id).parent().append(`<span class="text-danger fontError">${errorMessage}</span>`)
                        //console.log("Error message:", errorMessage);  // Each error message for this field
                    });
                }
            }
            //errorsJs = null
        }    
        erorsCheck()
        // show modal dialog if it first 
        var showDialog = () => {
            function isEmptyObject(obj) {

                return Object.keys(obj).length === 0 && obj.constructor === Object;
            }
           
            if (sessionStorage.getItem("return") !== ""){
                if (isEmptyObject(errorsJs))
                    $('#staticBackdrop').modal('show');
               

            }
        }
        showDialog()
    })
 
var organizationFildToggle = function () {
    $("#organizationField").hasClass("invisible")
        ? $("#organizationField").removeClass("invisible").addClass("visible")
        : $("#organizationField").removeClass("visible").addClass("invisible")
}
var addFatherName = function (num, model) {
    return `
                <div class="col-12 col-md-4">
                    <input asp-for="Performances[${num}].${model}.PersonFatherName"
                        name="Performances[${num}].${model}.PersonFatherName"
                        class="form-control fontSimple"
                        aria-describedby="Person Father Name"
                        id="Performances_${num}_${model}PersonFatherName"
                        placeholder="По батькові" />
                    <span asp-validation-for="Performances[${num}].${model}.PersonFatherName" class="text-danger fontError d-block"></span>
                </div>`
}