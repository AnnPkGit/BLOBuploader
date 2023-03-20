import React, { Component } from 'react';
import './Home.css';

export class Home extends Component {
    static displayName = Home.name;
    fileRef = React.createRef();

    constructor(props) {
      super(props);

      this.state = {
          file: null,
          email: '',
          res: localStorage.getItem('res')
        };
      localStorage.setItem('res', '');
    }

    fileChange = (e) => {
        let fileName = e.target.files[0].name;
        let dotIndex = fileName.lastIndexOf(".");
        let extension = fileName.substring(dotIndex);

        if (extension != '.docx') {
            this.setState({ res: 'Unsupported file format' });
            this.fileRef.current.value = '';
            return;
        }
        this.setState({ res: '' });

        var formData = new FormData();
        formData.append('docxFile', e.target.files[0]);
        this.setState({file: formData});
    };

    emailChange = (e) => {
        this.setState({ email: e.target.value });
    };

    sendFile = () => {
        if (this.state.email == '') {
            this.setState({ res: 'Email can not be empty' });
            return;
        }
        if (this.state.file == null) {
            this.setState({ res: 'File can not be empty' });
            return;
        }

        localStorage.setItem('res', '');
        fetch(`blob//docx?email=${this.state.email}`, {
            method: 'POST',
            body: this.state.file
        }).then(r => r.text())
          .then(t => localStorage.setItem('res', t))
          .then(() => document.location.reload());
    }

  render() {
    return (
        <div className="centerDiv">
            <div>
                <div>
                    <h3>Type your email</h3>
                    <input className="inputStyle" type="email" onChange={this.emailChange} />
                </div>
                <div>
                    <h3>Choose a file</h3>
                    <input className="inputStyle" type="file" onChange={this.fileChange} accept=".docx" ref={ this.fileRef }/>
                </div>
                <div>
                    <button className="inputBtn" onClick={this.sendFile}>Send file to the BLOB Storage</button>
                </div>
                <div>
                    <label value={this.state.res}>{this.state.res}</label>
                </div>
            </div>            
      </div>
    );
  }
}
