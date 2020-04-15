$(document).on("click", "#AddAvailableValues", function (e) {
  e.preventDefault();
  $.ajax({
      type: "POST",
      cache:false,
      url: $(this).attr("href"),
      data: $("form").serialize(),
      success: function (data) {
          var html = $(data).find("#tAvailableValues");
          $("#dAvailableValues").html("").html(html[0].outerHTML);
      }
  })
});

$(document).on("click", ".remove-value", function (e) {
  e.preventDefault();
  var tr = $(e.target).parents("tr");
  $(tr).find("input.deleted-value").val(true);
  $(tr).hide();
})

$(document).on("click", "#AddDefaultValues", function (e) {
  e.preventDefault();
  $.ajax({
      type: "POST",
      cache: false,
      url: $(this).attr("href"),
      data: $("form").serialize(),
      success: function (data) {
          if (data !== "100") {
              var html = $(data).find("#tDefaultValues");
              $("#dDefaultValues").html("").html(html[0].outerHTML);
          }
          else {
              alert("Parameter cannot have more than one default value if it does not allow multiple values.");
          }
      }
  })
});

$(document).on("click", ".remove-default", function (e) {
  e.preventDefault();
  var tr = $(e.target).parents("tr");
  $(tr).find("input.deleted-default").val(true);
  $(tr).hide();
})