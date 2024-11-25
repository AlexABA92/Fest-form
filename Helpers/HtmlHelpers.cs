
using Fest_form.data.Entity;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;

using System;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Fest_form.Helpers
{
    public static class HtmlHelpers
    {


        public static IHtmlContent PerformanceReturn(this IHtmlHelper htmlHelper,
            int index,
            Performance performance,
            List<Genre> genreList,
            List<Category> categoryList,
            List<ParticipantsNumber> participantsList)
        {

           

            var performanceItem = new TagBuilder("div");
            
            performanceItem.Attributes["id"] = $"per{index}";
            performanceItem.AddCssClass($"performance performance{index} border-top border-1 border-secondary m-1 p-1");
            #region PerformanceName
            performanceItem.InnerHtml.AppendHtml($"<div>" +
                $"<div class=\"form-group  text-start mb-2\">" +
                $"<label asp-for=\"Performances[{index}].PerformanceName\" class=\"control-label mb-2\">{Resources.Resource.PerformanceName} :</label>" +
                $"<input asp-for=\"Performances[{index}].PerformanceName\"" +
                $"name=\"Performances[{index}].PerformanceName\"" +
                $"value=\"{performance.PerformanceName}\" " +
                $"type=\"text\" " +
                $"class=\"form-control fontSimple\"" +
                 $"id=\"Performances_{index}_PerformanceName\"" +
                $"aria-describedby={Resources.Resource.PerformanceName} />" +
               
                $"<span asp-validation-for=\"Performances[{index}].PerformanceName\" class=\"text-danger fontError\"></span>" +
                $"</div>" +
                $" </div>"
                );
            #endregion

            #region ChoreographerDirector
            var chDirPerBlock = new TagBuilder("div");
            chDirPerBlock.AddCssClass("form-group mb-3 text-start");
            chDirPerBlock.InnerHtml.AppendHtml($"<label class=\"control-label mb-2\">{Resources.Resource.ChoreographerDirector} :</label>");

            chDirPerBlock.InnerHtml.AppendHtml(
                    Person(index,"ChoreographerDirector",
                    performance.ChoreographerDirector
                    ));

            performanceItem.InnerHtml.AppendHtml(
               chDirPerBlock);
            #endregion

            #region Concertmaster
            var concertMeister = new TagBuilder("div");
            concertMeister.AddCssClass("form-group mb-3 text-start");
            concertMeister.InnerHtml.AppendHtml($"<label class=\"control-label mb-2\">{Resources.Resource.Concertmaster} :</label>");

            concertMeister.InnerHtml.AppendHtml(
                   Person(index,"Concertmaster",
                  performance.Concertmaster
                   ));

            performanceItem.InnerHtml.AppendHtml(concertMeister);
            #endregion

            #region ParticipantsNumber
            var participantsAllBlock = new TagBuilder("div");
            participantsAllBlock.Attributes["id"] = $"personListforNum{index}";
            var participantsNumberSelectBlock = new TagBuilder("div");
            participantsNumberSelectBlock.AddCssClass($"container p-0 col-md-6");
            participantsNumberSelectBlock.InnerHtml.AppendHtml(
                $"<label for=\"parNum{index}\" >{Resources.Resource.ParticipantsNameListLabel} : </label>");
            var participantsNumberSelect = new TagBuilder("select");
            participantsNumberSelect.AddCssClass("form-select text-center fontSimple fw-bold numSelect mt-1");
            participantsNumberSelect.Attributes["id"] = $"Performances_{index}_ParticipantsNumberId";
            participantsNumberSelect.Attributes["name"] = $"Performances[{index}].ParticipantsNumberId";
            participantsNumberSelect.Attributes["data-id"] = index.ToString();

            if (participantsList != null)
                if (performance.ParticipantsNumberId == 0)
                {
                    var option = new TagBuilder("option");
                    option.Attributes["value"] = "";
                    option.Attributes["selected"] = $"selected";
                    option.InnerHtml.Append("--------");
                    participantsNumberSelect.InnerHtml.AppendHtml(option);
                }
            foreach (var item in participantsList)
                {
                    var option = new TagBuilder("option");
                    option.Attributes["value"] = $"{item.Id}";
                    option.InnerHtml.Append($"{Resources.Resource.ResourceManager.GetString(item.Name)}");
                    if (performance.ParticipantsNumberId == item.Id)
                        option.Attributes["selected"] = $"selected";
                    participantsNumberSelect.InnerHtml.AppendHtml(option);
                }
            participantsNumberSelectBlock.InnerHtml.AppendHtml(participantsNumberSelect);
            participantsAllBlock.InnerHtml.AppendHtml(participantsNumberSelectBlock);
           
            var persCount = Math.Abs((int)performance.ParticipantsNumberId);
            if (persCount <= 3 &&
                persCount >= 1)
            {
                var personlistBlock = new TagBuilder("div");
                personlistBlock.Attributes["id"] = $"personList{index}";
                for (int i = 0; i < persCount; i++)
                {
                    Person? personNum = i switch
                    {
                        0 => performance.ParticipantsNameList.Person1,
                        1 => performance.ParticipantsNameList.Person2,
                        2 => performance.ParticipantsNameList.Person3,
                        _ => null
                    };
                    var personItem = new TagBuilder("div");
                    personItem.AddCssClass("form-group mb-3 text-start");
                    personItem.InnerHtml.AppendHtml($"<label class=\"control-label mb-2\">{Resources.Resource.ParticipantNumList} :</label>");

                    personItem.InnerHtml.AppendHtml(
                           Person(index,$"ParticipantsNameList.PersonRepos{i + 1}",
                          personNum!
                           ));
                    personlistBlock.InnerHtml.AppendHtml(personItem);
                }
                participantsAllBlock.InnerHtml.AppendHtml(personlistBlock);
            }
            #endregion

            var selectsBlock = new TagBuilder("div");
            selectsBlock.AddCssClass("row row-cols-sm-2 mt-2");
            #region PerformanceGroup
            var genreSelectBlock = new TagBuilder("div");
            genreSelectBlock.InnerHtml.AppendHtml(
                $" <label for=\"floatingSelect{index}1\" id=\"genreLabel{index}\">{Resources.Resource.GenreLabel} : </label>");
               var genreselect = new TagBuilder("select");
            genreselect.AddCssClass("form-select text-center fontSimple fw-bold mt-1");
            genreselect.Attributes["data-id"] = index.ToString();
            genreselect.Attributes["id"] = $"Performances_{index}_GenreId";
            genreselect.Attributes["name"] =$"Performances[{ index}].GenreId";

            if (genreList != null)
                if (performance.GenreId == 0) {
                    var option = new TagBuilder("option");
                    option.Attributes["value"] = "";
                    option.Attributes["selected"] = $"selected";
                    option.InnerHtml.Append("--------");
                    genreselect.InnerHtml.AppendHtml(option);
                }
                foreach (var item in genreList) {
                   var option = new TagBuilder("option");
                    option.Attributes["value"] = $"{item.Id}";
                    option.InnerHtml.Append($"{Resources.Resource.ResourceManager.GetString(item.Name)}");
                    if (performance.GenreId == item.Id)
                        option.Attributes["selected"] = $"selected";
                    genreselect.InnerHtml.AppendHtml(option);
                }
            genreSelectBlock.InnerHtml.AppendHtml(genreselect);
            #endregion
            
            #region ageGroup
            var ageSelectBlock = new TagBuilder("div");
            ageSelectBlock.InnerHtml.AppendHtml(
              $" <label for=\"floatingSelect{index}2\" id=\"ageGroup{index}\">{Resources.Resource.AgeGroup} : </label>");
            var ageSelect = new TagBuilder("select");
            ageSelect.AddCssClass("form-select text-center fontSimple fw-bold mt-1");
            ageSelect.Attributes["data-id"] = index.ToString();
            ageSelect.Attributes["id"] = $"Performances_{index}_PerformanceGroupId";
            ageSelect.Attributes["name"] = $"Performances[{index}].CategoryId";
            if (categoryList != null)
                if (performance.CategoryId == 0)
                {
                    var option = new TagBuilder("option");
                    option.Attributes["value"] = "";
                    option.Attributes["selected"] = $"selected";
                    option.InnerHtml.Append("--------");
                    ageSelect.InnerHtml.AppendHtml(option);
                }
            foreach (var item in categoryList)
                {
                    var option = new TagBuilder("option");
                    option.Attributes["value"] = $"{item.Id}";
                    option.InnerHtml.Append($"{Resources.Resource.ResourceManager.GetString(item.Name)} - " +
                        $"{Resources.Resource.ResourceManager.GetString(item.Description)}");
                    if (performance.CategoryId == item.Id)
                        option.Attributes["selected"] = $"selected";
                    ageSelect.InnerHtml.AppendHtml(option);
                }
            ageSelectBlock.InnerHtml.AppendHtml(ageSelect);

            #endregion


            selectsBlock.InnerHtml.AppendHtml(genreSelectBlock);
            selectsBlock.InnerHtml.AppendHtml(ageSelectBlock);
           
            

            performanceItem.InnerHtml.AppendHtml(participantsAllBlock);
            performanceItem.InnerHtml.AppendHtml(selectsBlock);

            #region timeSpot
             var timeSpotAllBlock = new TagBuilder("div");
                timeSpotAllBlock.AddCssClass("row justify-content-around");
            var wingCheced = performance.StartPoint == GlobalData.Enum.StartPointEnum.Wing ? "checked" : "";
            var spotCheced = performance.StartPoint == GlobalData.Enum.StartPointEnum.Point ? "checked" : "";
            timeSpotAllBlock.InnerHtml.AppendHtml(
                $" <div class=\"col-6\">" +
                    $"<label for=\"time{index}\">{Resources.Resource.PerfomanseTime} : </label>" +
                    $"<input type =\"time\" " +
                        $"id=\"Performances_{index}_PerformanceTime\" " +
                        $"asp-for=\"Performances[{index}].PerformanceTime\"" +
                        $"class=\"form-control col-12 col-md-3 text-center\" " +
                        $"name=\"Performances[{index}].PerformanceTime\"" +
                        $"value=\"{performance.PerformanceTime}\"/>" +
                    $"<span asp-validation-for=\"Performances[{index}].PerformanceTime\" class=\"text-danger fontError\"></span>" +
                $"</div>" +
                $"<div class=\"col-6 \">" +
                    $"<label for=\"startSpot{index}\"\">{Resources.Resource.StartPosition} : </label>" +
                    $" <div class=\"row justify-content-md-around justify-content-center\">" +
                        $"<input type = \"radio\" class=\"btn-check\" asp-for=\"Performances[{index}].StartPoint\"" +
                        $"name=\"Performances[{index}].StartPoint\" value=\"Wing\" id=\"btn-check-outlined{index}0\" autocomplete=\"off\"" +
                        $" {wingCheced}/>" +
                        $"<label class=\"btn btn-outline-primary col-8 mb-1 col-md-4 fontSimple\"" +
                        $" for=\"btn-check-outlined{index}0\" id=\"radio{index}1\">{Resources.Resource.Backstage}</label>" +
                        $"<input type =\"radio\" class=\"btn-check\" asp-for=\"Performances[{index}].StartPoint\"" +
                        $"name=\"Performances[{index}].StartPoint\" value=\"Point\" id=\"btn-check-outlined{index}1\" autocomplete=\"off\" {spotCheced}/>" +
                        $"<label class=\"btn btn-outline-primary col-8 mb-1 col-md-4 fontSimple\"" +
                        $"for=\"btn-check-outlined{index}0\" id=\"radio{index}2\">{Resources.Resource.Spot}</label>" +
                    $"</div>" +
                 $"</div>");
            performanceItem.InnerHtml.AppendHtml(timeSpotAllBlock);
            #endregion
            #region file
            var divFile = new TagBuilder("div");
            divFile.AddCssClass("form-group text-start mb-2 container");
            divFile.InnerHtml.AppendHtml($"<label for=\"file{index}\">{Resources.Resource.FileAdd} :</label>" +
                $" <input type =\"file\" name=\"Performances[{index}].PhonogramFileURL\"" +
                $" id=\"Performances_{index}_PhonogramFileURL\" class=\"form-control fontSimple\" accept=\".mp3, .wav\"/>");
            performanceItem.InnerHtml.AppendHtml(divFile);
            #endregion
            #region youtube
            var youtubeDiv = new TagBuilder("div");
            youtubeDiv.AddCssClass("form-group  text-start mb-2 container");
            youtubeDiv.InnerHtml.AppendHtml($"<label asp-for=\"Performances[{index}].YouTubeVideoURL\" class=\"control-label  mb-2\" " +
                $">{Resources.Resource.YouTubeURL} :</label>" +
                $"<input asp-for=\"Performances[{index}].YouTubeVideoURL\"" +
                $"name=\"Performances[{index}].YouTubeVideoURL\"" +
                $"id=\"Performances_{index}_YouTubeVideoURL\"" +
                $"value=\"{performance.YouTubeVideoURL}\""+
                $" class=\"form-control fontSimple\" aria-describedby=\"{Resources.Resource.YouTubeURL}\" " +
                $"<span asp-validation-for=\"Performances[{index}].YouTubeVideoURL\" class=\"text-danger  fontError\"></span>");
            #endregion
            performanceItem.InnerHtml.AppendHtml(youtubeDiv);
            performanceItem.InnerHtml.AppendHtml(
                "<div class=\"text-end me-3\">" +
                "<button class=\"btn btn-danger delete  m-2\"" +
                $"type=\"button\" data-id=\"{index}\">" +
                "<i class=\"bi bi-trash\"></i>" +
                "</button>" +
                "</div>" +
                "</div>");


            return performanceItem;
        }

        public static IHtmlContent PersonReturn(this IHtmlHelper htmlHelper, string model, Person person) {
                     
            return Person(0, model, person);

        }
       static TagBuilder  Person(int index, string model, Person person)
        {
            var personDiv = new TagBuilder("div");
            personDiv.AddCssClass("form-group row align-items-center");
            personDiv.InnerHtml.AppendHtml(
            $"<div class=\"col-12 col-md-4\">" +
                    $"<input asp-for=\"Performances[{index}].{model}.PersonLastName\"" +
                    $"name=\"Performances[{index}].{model}.PersonLastName\"" +
                    $"id=\"Performances_{index}_{model.Replace(".", "")}PersonLastName\"" +
                    $"value=\"{person.PersonLastName}\"" +
                    $"class=\"form-control fontSimple mb-1\"" +
                    $"aria-describedby=\"PersonRepos Last Name\"" +
                    $"placeholder=\"{Resources.Resource.PersonLastNameLable}\"/>" +
                    $"<span asp-validation-for=\"Performances[{index}].{model}.PersonLastName\" class=\"text-danger fontError d-block\"></span>" +
                $"</div>" +
            $"<div class=\"col-12 col-md-4\">\r\n" +
                    $"<input asp-for=\"Performances[{index}].{model}.PersonName\"\r\n" +
                    $"id=\"Performances_{index}_{model.Replace(".", "")}PersonName\"" +
                    $"name=\"Performances[{index}].{model}.PersonName\" " +
                    $"value=\"{person.PersonName}\"" +
                    $"class=\"form-control fontSimple mb-1\"\r\n" +
                    $"aria-describedby=\"PersonRepos Name\"\r\n" +
                    $"placeholder=\"{Resources.Resource.PersonFirstNameLable}\" />\r\n" +
                    $"<span asp-validation-for=\"Performances[{index}].{model}.PersonName\" class=\"text-danger fontError d-block\"></span>\r\n" +
                $"</div>"
            );
            if (!string.IsNullOrEmpty(person.PersonFatherName))
                personDiv.InnerHtml.AppendHtml(
                $" <div class=\"col-12 col-md-4\">" +
                        $"<input asp-for=\"Performances[{index}].{model}.PersonFatherName\"" +
                        $"id=\"Performances_{index}_{model.Replace(".", "")}PersonFatherName\"" +
                        $"class=\"form-control fontSimple\"" +
                        $"aria-describedby=\"PersonRepos Father Name\"" +
                        $"name=\"Performances[{index}].{model}.PersonFatherName\"" +
                        $"value=\"{person.PersonFatherName}\"" +
                        $"placeholder=\"По батькові\" />" +
                    $"</div>");
            return personDiv;
        }
    }
}
