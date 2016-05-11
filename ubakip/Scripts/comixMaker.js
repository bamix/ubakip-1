angular.module('MyApp', ['ngDialog']).controller('comicsMakerController', function ($scope, ngDialog) {
    var RotationAngle = 0,
    Scale = 1,
    PosX = 0,
    PosY = 0,
    ImageId = null,
    height = 0,
    width = 0,
    isDraging = false,
    isMinScale = false,
    divHeight = 0,
    divWidth = 0,
    StraightType = {
        Vertical: { value: 0 },
        Horizontal: { value: 1 },
        Custom: { value: 2 }
    },
    isFirstLoad = true,
    countLoadedImages = 0,
    EmptyCloud = { id: null, type: null, text: "", posX: 0, posY: 0, width: 0, height: 0 };

    $scope.background = "000000";

    $scope.images = [
    //    {
    //    src: "https://pp.vk.me/c630516/v630516851/17d3a/o2M3HScGpQc.jpg",
    //    isVideo: false,
    //    scale: 1,
    //    rotate: 0,
    //    posX: 0,
    //    posY: 0,
    //    cellId: "sq1",       
    //    id: "1",
    //    height:0,
    //    width : 0
    //},
    //    {
    //    src: "https://pp.vk.me/c630516/v630516851/17d3a/o2M3HScGpQc.jpg",
    //    isVideo: false,
    //    scale: 1,
    //    rotate: 90,
    //    posX: 0,
    //    posY: 0.4,
    //    cellId: "sq2",        
    //    id: "2",
    //    height: 0,
    //    width: 0
    //},
    //    {
    //    src: "http://clips.vorwaerts-gmbh.de/VfE_html5.mp4",
    //    isVideo : true,
    //    scale: 2,
    //    rotate: 0,
    //    posX: 0,
    //    posY: 0,
    //    cellId: "sq3",     
    //    id: "3",
    //    height: 0,
    //    width: 0
    //}
    ];

    $scope.clouds = [
     //   {
     //       id: 0,
     //       type: "cloud1",
     //       text: "lol",
     //       posX: 0,
     //       posY: 0,
     //       scaleX: "50%",
     //       scaleY: "50%"
     //   },
     //{
     //    id: 1,
     //    type: "cloud2",
     //    text: "kek",
     //    posX: 0.2,
     //    posY: 0.1,
     //    scaleX: "30%",
     //    scaleY: "30%"
     //}
    ];

    $scope.template = "template1";



    interact.maxInteractions(Infinity);   // Allow multiple interactions

    interact('.image-cell')
    .draggable({
        autoScroll: true,
        onmove: dragMoveListener,
        onend: function (event) {
            function dragEndDelay() {
                isDraging = false;
            }
            setTimeout(dragEndDelay, 100);
        }
    })
     .on('hold', LoadDialog);

    $scope.Initialize = function (model) {
        if (model != null) {
            $scope.images = model.imageCell;
            $scope.clouds = model.clouds;
            $scope.template = model.templateName;
            $scope.background = model.background;
        }
        LoadTemplate();
    }

    interact('.background-cell')
     .on('hold', LoadDialog);

    interact('.cloud')
    .draggable({
        onmove: function (event) {
            var rect = event.target.getBoundingClientRect(),
                cloudId = -event.target.id - 1;
            $scope.clouds[cloudId].posX = $scope.clouds[cloudId].posX + event.dx / rect.width;
            $scope.clouds[cloudId].posY = $scope.clouds[cloudId].posY + event.dy / rect.height;
            MoveCloud(cloudId);
        }
    })
    .resizable({
        preserveAspectRatio: false,
        edges: { left: false, right: true, bottom: true, top: false },
        onstart: CloudResizeStartListner,
        onmove: CloudResizeMoveListner,
        onend: CloudResizeEndListner
    })
    .on('doubletap', DeleteCloud);

    interact('.slider')
      .origin('self')
      .draggable({
          max: 1,
          restrict: {
              restriction: 'self',
          },
          inertia: true
      })
      .on('dragmove', function (event) {
          var sliderWidth = interact.getElementRect(event.target.parentNode).width,
              value = event.pageX / sliderWidth;
          if (event.target.id == "slider_rotate") {
              SliderRotateListner(event, value);
          }
          if (event.target.id == "slider_scale") {
              SliderScaleListner(event, value)
          }
      });

    function LoadDialog(event) {
        ngDialog.open({
            template: 'Enter URL:  <input type="url" id="url"/><br/>or <input id="file" type="file"><br/><button type="button" ng-click="AddIm()" class="btn btn-success">Add</button>',
            plain: true,
            className: 'ngdialog-theme-default',
            controller: ['$scope', function ($scope) {
                $scope.AddIm = function () {
                    if ($("#file").val() != "") { alert($("#file").val()); }
                    if ($("#url").val() != "") { alert("kek2"); }
                    ngDialog.close();
                }
            }],
            scope: $scope
        });
    }

    $("#main-form").delegate(".video-btn", "click", function () {
        playPause($(this).parent().find("video"), $(this));
    });

    function playPause(myVideo, image) {
        if (myVideo.get(0).paused) {
            myVideo.get(0).play();
            image.attr("src", "../../Content/Images/pause.png");
        }
        else {
            myVideo.get(0).pause();
            image.attr("src", "../../Content/Images/play.png");
        }
    }

    $("#main-form").delegate(".cell .image-cell", "click", function () {
        var Imgid = $(this).attr('id');
        if (ImageId == Imgid || isDraging) { return; }
        ImageId = Imgid;
        SetTransformFromImage($scope.images[ImageId]);
        EnableSliders();
    });

    $("#main-form").delegate(".textarea", "input", function () {
        $scope.clouds[-$(this).parent().attr("id") - 1].text = $(this).val();
    });

    document.body.addEventListener('load', function (event) {//for image load
        InitElement(event.target);
    }, true);

    document.body.addEventListener('loadedmetadata', function (event) {//for video load
        InitElement(event.target);
    }, true);

    function InitElement(elm) {
        if (!$(elm).hasClass("image-cell")) return;
        SetImageHeightAndWidth(elm);
        divHeight = $(elm).parent().height();
        divWidth = $(elm).parent().width();
        if (isFirstLoad) {
            SetTransformFromImage($scope.images[elm.id]);
            Transform($(elm));
        }
        else {
            ImageId = elm.id;
            SetTransform(0, 1, 0, 0);
            Transform($(elm));
            ImageId = null;
        }
        countLoadedImages--;
        if (countLoadedImages == 0) isFirstLoad = false;
    }

    $(".btn").click(function () {
        var id = $(this).attr("id");
        if (id == "save") {
            var page = {
                Id: "0",
                ImageCell: $scope.images,
                Clouds: $scope.clouds,
                TemplateName: $scope.template,
                Background: $scope.background,
                Preview: $("#main-form").html()
            }

            $.ajax({
                type: 'POST',
                url: "/Comix/SavePage",
                dataType: 'json',
                data: JSON.stringify(page),
                contentType: "application/json; charset=utf-8",
                traditional: true,
                success: function (data) {
                    alert("all good")
                }
            });
        }
    });

    $(".btn-template").click(function () {
        $scope.template = $(this).attr("id");
        LoadTemplate();
    });

    $(".btn-cloud").click(function () {
        var id = $(this).attr("id"),
            CloudId = $scope.clouds.length + 1;
        AddCloud(CloudId, id, "");
        $scope.clouds.push({ id: CloudId, type: id, text: "", posX: 0, posY: 0, width: "20%", height: "20%" });
    });

    window.onload = function () {
        $(".cell").bind("dragover", function (e) {
            if (e.preventDefault) e.preventDefault();
            if (e.stopPropagation) e.stopPropagation();
        });

        $(".cell").bind("dragenter", function (e) {

        });

        $(".cell").bind("dragleave", function (e) {

        });

        $(".cell").bind("drop", function (e) {
            if (e.preventDefault) e.preventDefault();
            if (e.stopPropagation) e.stopPropagation();
            var file = event.dataTransfer.files[0];
            alert($(this).attr("id"));
        });
    };

    function LoadTemplate() {
        $.ajax({
            type: 'POST',
            url: "/Comix/LoadTemplate",
            data: { id: $scope.template },
            success: function (data) {
                $("#main-form").empty();
                $("#main-form").append(data);
                LoadImages();
                LoadCellBackground();
                LoadClouds();
            }
        });
    }

    function LoadImages() {
        $($scope.images).each(function (index, value) {
            if ($('#' + value.cellId).length) {
                $('#' + value.cellId).empty();
                if (value.isVideo) {
                    $('#' + value.cellId).append(
                        '<img class="video-btn" src="../../Content/Images/play.png" />' +
                    '<video id=' + index + '  class= "image-cell">' +
                    ' <source src="' + value.src + '" type="video/mp4">  ' +
                    ' Your browser does not support the video tag.</video>'
                    );
                }
                else {
                    $("<img/>", {
                        id: index,
                        src: value.src,
                        class: "image-cell"
                    }).appendTo($('#' + value.cellId));
                }
                countLoadedImages++;
            }
        });
    }

    function LoadCellBackground() {
        $(".cell").each(function (index, item) {
            if ($(item).is(':empty')) {
                $(item).append("<div class='background-div dropable'> <img draggable = 'false' class='background-cell'" +
                    " src='../../Content/Images/backgroundcell.jpg'/></div>");
            }
        });

    }

    function LoadClouds() {
        $($scope.clouds).each(function (index, value) {
            AddCloud(index + 1, value.type, value.text);
            MoveCloud(index);
        });
    }

    function AddCloud(id, border, text) {
        $(".main-form").append(' <div class="cloud" id="-' + id + '"><textarea class="textarea" id="-t' + id + '" name="text"' +
                   'style="-webkit-mask-box-image: url(\'../../Content/Images/' + border +
                   '.png\'); mask-border: url(\'../../Content/Images/' + border + '.png\') ;">' + text + '</textarea></div>');
    }

    function CloudResizeStartListner(event) {
        var target = event.target,
        cloudId = -event.target.id - 1,
        oldCloudPosition = $("#" + target.id).position();
        $("#" + event.target.id).css(
        {
            'webkittransform': "translate(" + oldCloudPosition.left + "px, " + oldCloudPosition.top + "px)",
            'transform': "translate(" + oldCloudPosition.left + "px, " + oldCloudPosition.top + "px)"
        });
    }

    function CloudResizeMoveListner(event) {
        var target = event.target,
            cloudId = -event.target.id - 1,
            containerRect = document.getElementById("main-form").getBoundingClientRect();
        $scope.clouds[cloudId].width = target.style.width = event.rect.width / containerRect.width * 100 + '%';
        $scope.clouds[cloudId].height = target.style.height = event.rect.height / containerRect.height * 100 + '%';
    }

    function CloudResizeEndListner(event) {
        var target = event.target,
            cloudId = -event.target.id - 1,
            CloudPosition = $(target).position(),
            CloudRect = target.getBoundingClientRect();
        $scope.clouds[cloudId].posX = (CloudPosition.left) / CloudRect.width;
        $scope.clouds[cloudId].posY = (CloudPosition.top) / CloudRect.height;
        MoveCloud(cloudId);
    }

    function MoveCloud(index) {
        $("#" + (-index - 1)).css(
            {
                'height': $scope.clouds[index].height,
                'width': $scope.clouds[index].width,
                'webkittransform': "translate(" + $scope.clouds[index].posX * 100 + "%, " + $scope.clouds[index].posY * 100 + "%)",
                'transform': "translate(" + $scope.clouds[index].posX * 100 + "%, " + $scope.clouds[index].posY * 100 + "%)",

            });
    }

    function SliderRotateListner(event, value) {
        if (ImageId == null) return;
        var newValue = value * 359;
        SetSliderValue(event.target, value, newValue.toFixed(0));
        RotationAngle = newValue;
        $scope.images[ImageId].rotate = RotationAngle;
        Transform($("#" + ImageId));
        FixRotate();
    }

    function SliderScaleListner(event, value) {
        if (ImageId == null) return;
        var newValue = value * 9 + 1;
        if (!TryChangeScale(newValue, event.target, value)) {
            if (isMinScale) return;
            isMinScale = true;
            MoveToCenter();
            TryChangeScale(newValue, event.target, value);
        } else { isMinScale = false; }
    }

    function SetImageHeightAndWidth(elm) {
        if ($(elm).height() / $(elm).parent().height() > $(elm).width() / $(elm).parent().width()) {
            $(elm).css({ 'height': 'auto', 'width': '100%' });
        }
        else {
            $(elm).css({ 'height': '100%', 'width': 'auto' });
        }
        $scope.images[elm.id].height = $(elm).height();
        $scope.images[elm.id].width = $(elm).width();
    }

    function DeleteCloud(event) {
        var parent = $("#" + event.target.id).closest('div');
        $scope.clouds[-parent.attr("id") - 1] = EmptyCloud;
        parent.remove();
    }

    function MoveToCenter() {
        var cellCenter = GetCenterElementByID($("#" + ImageId).parent().attr("id")),
            imageCenter = GetCenterElementByID(ImageId),
            newPosition = RotateBasis((cellCenter.x - imageCenter.x) / Scale, (cellCenter.y - imageCenter.y) / Scale, RotationAngle)
        SetPosition(PosX + newPosition.x / width, PosY + newPosition.y / height);
        Transform($("#" + ImageId));
    }

    function TryChangeScale(newValue, slider, value) {
        if (fixScale(newValue)) {
            SetSliderValue(slider, value, newValue.toFixed(2));
            $scope.images[ImageId].scale = Scale;
            Transform($("#" + ImageId));
            return true;
        }
        return false;
    }

    function SetSliderValue(element, offset, value) {
        element.style.paddingLeft = (offset * 100) + '%';
        element.setAttribute('data-value', value);
    }

    function Transform(element) {
        element.css(
            {
                'webkitTransform': "rotate(" + RotationAngle + "deg) scale(" + Scale + ")" +
        "translate(" + PosX.toFixed(5) * 100 + "%, " + PosY.toFixed(5) * 100 + "%)",
                'transform': "rotate(" + RotationAngle + "deg) scale(" + Scale + ")" +
        "translate(" + PosX.toFixed(5) * 100 + "%, " + PosY.toFixed(5) * 100 + "%)"
            });
    }

    function SetTransform(rotationAngle, scale, posX, posY) {
        $scope.images[ImageId].scale = Scale = scale;
        $scope.images[ImageId].rotate = RotationAngle = rotationAngle;
        $scope.images[ImageId].posX = PosX = posX;
        $scope.images[ImageId].posY = PosY = posY;
    }

    function SetTransformFromImage(image) {
        Scale = image.scale;
        RotationAngle = image.rotate;
        PosX = image.posX;
        PosY = image.posY;
    }

    function EnableSliders() {
        var rotate = $scope.images[ImageId].rotate,
            scale = $scope.images[ImageId].scale;
        SetSliderValue(document.getElementById("slider_rotate"), rotate / 359, rotate.toFixed(0));
        SetSliderValue(document.getElementById("slider_scale"), (scale - 1) / 9, scale.toFixed(2));
    }

    function dragMoveListener(event) {
        if (!isDraging) {
            ImageId = event.target.id;
            SelectWorkImage();
        }
        isDraging = true;
        isMinScale = false;
        delta = { dx: event.dx, dy: event.dy };
        var deltas = FixDeltas(delta);
        if (deltas.dx != 0 || deltas.dy != 0) {
            Move(deltas);
        }
    }

    function SelectWorkImage() {
        SetTransformFromImage($scope.images[ImageId]);
        EnableSliders();
        var divRect = $("#" + ImageId).parent().get(0).getBoundingClientRect();
        divHeight = divRect.height;
        divWidth = divRect.width;
        height = $scope.images[ImageId].height;
        width = $scope.images[ImageId].width;
    }

    function CheckContaining(rect) {
        if (CheckVertex(rect.point1) && CheckVertex(rect.point2) &&
            CheckVertex(rect.point3) && CheckVertex(rect.point4)) {
            return true;
        }
        return false;
    }

    function FixDeltas(delta) {
        var primaryDy = delta.dy;
        var dx = delta.dx, dy = delta.dy;
        if (!CheckContaining(AddTranslateChanges(delta))) {
            delta.dy = 0; dy = 0;
            if (!CheckContaining(AddTranslateChanges(delta))) {
                delta.dy = primaryDy; dy = primaryDy;
                delta.dx = 0; dx = 0;
                if (!CheckContaining(AddTranslateChanges(delta))) {
                    dx = 0; dy = 0;
                }
            }
        }
        return { dx: dx, dy: dy };
    }

    function fixScale(newScale) {
        if (CheckContaining(AddScaleChanges(newScale))) {
            Scale = newScale; return true;
        }
        return false;
    }

    function FixRotate() {
        if (!CheckContaining(AddTranslateChanges({ dx: 0, dy: 0 }))) {
            MoveToCenter();
            var minScale = FindMinScale();
            if (minScale > Scale) {
                $scope.images[ImageId].scale = Scale = minScale;
                Transform($("#" + ImageId));
                SetSliderValue(document.getElementById("slider_scale"), (Scale - 1) / 9, Scale.toFixed(2));
            }
        }
    }

    function FindMinScale() {
        var diagonal = Math.sqrt(Math.pow(divHeight, 2) + Math.pow(divWidth, 2)),
            minLength = height < width ? height : width;
        return diagonal / minLength;
    }

    function AddScaleChanges(newScale) {
        var dx = $("#" + ImageId).width() * (newScale - Scale) / 4,
            dy = $("#" + ImageId).width() * (newScale - Scale) / 4,
            divRect = $("#" + ImageId).parent().get(0).getBoundingClientRect(),
            vertex1 = { x: divRect.left + dx, y: divRect.top + dy },
            vertex2 = { x: divRect.right - dx, y: divRect.top + dy },
            vertex3 = { x: divRect.right - dx, y: divRect.bottom - dy },
            vertex4 = { x: divRect.left + dx, y: divRect.bottom - dy };
        return { point1: vertex1, point2: vertex2, point3: vertex3, point4: vertex4 };
    }

    function AddTranslateChanges(delta) {
        var dx = delta.dx, dy = delta.dy,
            divRect = $("#" + ImageId).parent().get(0).getBoundingClientRect();
        var vertex1 = { x: divRect.left - dx, y: divRect.top - dy },
            vertex2 = { x: divRect.right - dx, y: divRect.top - dy },
            vertex3 = { x: divRect.right - dx, y: divRect.bottom - dy },
            vertex4 = { x: divRect.left - dx, y: divRect.bottom - dy };
        return { point1: vertex1, point2: vertex2, point3: vertex3, point4: vertex4 };
    }

    function CheckVertex(vertex) {
        var foo = Deltas(GetExtrimLeftPoint(RotationAngle), vertex);
        var bar = Deltas(GetExtrimRightPoint(RotationAngle), vertex);
        if (foo.delta1 >= 0 && foo.delta2 >= 0 && bar.delta1 <= 0 && bar.delta2 <= 0) return true;
        else return false;
    }

    function Deltas(point, vertex) {
        var Straight = GetStraightParams(point.x, point.y, -RotationAngle % 90 - 90), delta1 = 0, delta2 = 0;
        var Straight2 = GetStraightParams(point.x, point.y, -RotationAngle % 90);
        if (Straight.type == StraightType.Vertical) {
            delta1 = vertex.x - point.x;
            delta2 = point.y - vertex.y;
        }
        else if (Straight.type == StraightType.Horizontal) {
            delta1 = point.y - vertex.y;
            delta2 = vertex.x - point.x;
        }
        else {
            delta1 = -(Straight.k * vertex.x - vertex.y + Straight.b) / Math.sqrt(Math.pow(Straight.k, 2) + Math.pow(Straight.b, 2));
            delta2 = (Straight2.k * vertex.x - vertex.y + Straight2.b) / Math.sqrt(Math.pow(Straight2.k, 2) + Math.pow(Straight2.b, 2));
        }
        return { delta1: delta1, delta2: delta2 };
    }

    function Move(deltas) {
        var newBasis = RotateBasis(deltas.dx, deltas.dy, RotationAngle);
        SetPosition(PosX + (newBasis.x / Scale) / width, PosY + (newBasis.y / Scale) / height);
        Transform($("#" + ImageId));
    }

    function SetPosition(posX, posY) {
        $scope.images[ImageId].posX = PosX = posX;
        $scope.images[ImageId].posY = PosY = posY;
    }

    function GetExtrimLeftPoint(angle) {
        if (angle < 90) return GetBottomLeftImagePoint();
        else if (angle < 180) return GetBottomRightImagePoint();
        else if (angle < 270) return GetTopRightImagePoint();
        else return GetTopLeftImagePoint();
    }

    function GetExtrimRightPoint(angle) {
        if (angle < 90) return GetTopRightImagePoint();
        else if (angle < 180) return GetTopLeftImagePoint();
        else if (angle < 270) return GetBottomLeftImagePoint();
        else return GetBottomRightImagePoint();
    }

    function GetTopLeftImagePoint() {
        return GetImagePoint(-width / 2, -height / 2);
    }

    function GetBottomLeftImagePoint() {
        return GetImagePoint(-width / 2, height / 2);
    }

    function GetTopRightImagePoint() {
        return GetImagePoint(width / 2, -height / 2);
    }

    function GetBottomRightImagePoint() {
        return GetImagePoint(width / 2, height / 2);
    }

    function GetImagePoint(offsetX, offsetY) {
        var offsetFirstPoint = RotateBasis(offsetX * Scale, offsetY * Scale, -RotationAngle);
        var center = GetCenterElementByID(ImageId);
        var x = center.x + offsetFirstPoint.x;
        var y = center.y + offsetFirstPoint.y;
        return { x: x, y: y };
    }

    function RotateBasis(x, y, angle) {
        var radian = angle * Math.PI / 180,
        newX = x * Math.cos(radian) + y * Math.sin(radian);
        newY = -x * Math.sin(radian) + y * Math.cos(radian);
        return { x: newX, y: newY };
    }

    function GetStraightParams(x, y, angle) {
        var k = 0, b = 0, type = StraightType.Custom;
        if (Math.abs(angle) % 180 == 0) {
            type = StraightType.Horizontal;
        }
        else if (Math.abs(angle) % 180 == 90) {
            type = StraightType.Vertical;
        }
        else {
            k = -Math.tan(angle * Math.PI / 180),
            b = y - k * x;
        }
        return { k: k, b: b, type: type }
    }

    function GetCenterElementByID(elementId) {
        var elemRect = document.getElementById(elementId).getBoundingClientRect(),
            x = elemRect.left + elemRect.width / 2,
            y = elemRect.top + elemRect.height / 2;
        return { x: x, y: y };
    }

    function GetDistance(point1, point2) {
        return Math.sqrt(Math.pow(point1.x - point2.x, 2) + Math.pow(point1.y - point2.y, 2));
    }

});
